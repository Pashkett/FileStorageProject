using AutoMapper;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using FileStorage.Data.FileSystemManagers.StorageFileManager;
using FileStorage.Data.FileSystemManagers.StorageFolderManager;
using FileStorage.Data.Models;
using FileStorage.Data.UnitOfWork;
using FileStorage.Domain.DataTransferredObjects.StorageItemModels;
using FileStorage.Domain.Utilities;
using FileStorage.Domain.DataTransferredObjects.UserModels;
using FileStorage.Logger;

namespace FileStorage.Domain.Services.StorageItemServices
{
    public class StorageItemsService : IStorageItemsService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IFolderManager folderManager;
        private readonly IFileManager fileManager;
        private readonly ILoggerManager loggerManager;
        private readonly string targetPath;
        private readonly long fileSizeLimit;

        public StorageItemsService(IUnitOfWork unitOfWork,
                                   IMapper mapper,
                                   IFolderManager folderManager,
                                   IFileManager fileManager,
                                   ILoggerManager loggerManager,
                                   IConfiguration configuration)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.folderManager = folderManager;
            this.fileManager = fileManager;
            this.loggerManager = loggerManager;
            targetPath = configuration.GetValue<string>("StoredFilesPath");
            fileSizeLimit = configuration.GetValue<long>("FileSizeLimit");
        }


        #region Files Operations
        public async Task CreateFileAsync(UserDto userDto, IFormFile file)
        {
            var user = mapper.Map<User>(userDto);
            var primaryFolder = await unitOfWork.StorageItems.GetUserPrimaryFolderAsync(user);

            if (primaryFolder == null)
            {
                loggerManager.LogError("User doesn't have primary folder");

                throw new ArgumentNullException("User doesn't have primary folder");
            }
            else
            {
                try
                {
                    var formFileContent = await file.ProcessFormFileAsync(fileSizeLimit);
                    var fileItem = file.CreateFileItemFormFile(primaryFolder, user);

                    string fullPath = StorageItemsHelpers.GetStorageItemFullPath(targetPath, fileItem.RelativePath);
                    await fileManager.CreateFileAsync(fullPath, formFileContent);

                    await unitOfWork.StorageItems.AddAsync(fileItem);
                    await unitOfWork.CommitAsync();
                }
                catch (Exception ex)
                {
                    loggerManager.LogError($"{ex.Message} \n{ex}");

                    throw ex;
                }
            }
        }

        public async Task<(MemoryStream stream, string contentType, string fileName)> DownloadFileAsync(
            UserDto userDto, string fileId)
        {
            var user = mapper.Map<User>(userDto);

            if (Guid.TryParse(fileId, out Guid storageItemId) == false)
                throw new ArgumentException($"{fileId} is not valid id");

            var fileItem = await unitOfWork.StorageItems.GetFileByUserAndFileIdAsync(user, storageItemId);

            if (fileItem == null)
                throw new ArgumentException($"File for current user does not exist.");

            var filePath = StorageItemsHelpers.GetStorageItemFullPath(targetPath, fileItem.RelativePath);

            var stream = await fileManager.ReadFileAsync(filePath);
            stream.Position = 0;

            return (stream, StorageItemsHelpers.GetContentType(filePath), fileItem.DisplayName);
        }

        #endregion

        #region Folders Operations
        public async Task<StorageItem> CreateFolderAsync(FolderCreateRequestDto folderCreate)
        {
            var folder = mapper.Map<FolderCreateRequestDto, StorageItem>(folderCreate);

            folder.TrustedName = StorageItemsHelpers.GetStorageItemTrustedName(folder);
            folder.RelativePath = StorageItemsHelpers.GetStorageItemRelativePath(folder);

            var existingFolderInDirectory =
                await unitOfWork.StorageItems.GetFolderByTrustedNameAsync(folder.TrustedName);

            string fullPath = StorageItemsHelpers.GetStorageItemFullPath(targetPath, folder.RelativePath);

            if (existingFolderInDirectory == null && !folderManager.IsFolderExists(fullPath))
            {
                await unitOfWork.StorageItems.AddAsync(folder);
                await unitOfWork.CommitAsync();

                folderManager.CreateFolder(fullPath);

                return folder;
            }
            else
                throw new ArgumentException("Folder has been already exists!");
        }

        public async Task DeleteFolderAsync(UserDto userDto, string id)
        {
            var user = mapper.Map<User>(userDto);

            if (Guid.TryParse(id, out Guid storageItemId) == false)
                throw new ArgumentException($"{id} is not valid id");

            var folderToDelete = 
                await unitOfWork.StorageItems.GetFolderByUserAndFolderIdIdAsync(user, storageItemId);

            if (folderToDelete == null)
                throw new ArgumentException("Folder doesn't exists");

            string fullPath = StorageItemsHelpers.GetStorageItemFullPath(targetPath, folderToDelete.RelativePath);

            if (!folderManager.IsFolderExists(fullPath))
                folderManager.DeleteFolder(fullPath);

            unitOfWork.StorageItems.Remove(folderToDelete);
            await unitOfWork.CommitAsync();
        }

        #endregion
    }
}
