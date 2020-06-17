using AutoMapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using FileStorage.Domain.DataTransferredObjects.StorageItemModels;
using FileStorage.Domain.DataTransferredObjects.UserModels;
using FileStorage.Domain.Utilities;
using FileStorage.Domain.Exceptions;
using FileStorage.Domain.PagingHelpers;
using FileStorage.Domain.RequestModels;
using FileStorage.Data.FileSystemManagers.StorageFileManager;
using FileStorage.Data.FileSystemManagers.StorageFolderManager;
using FileStorage.Data.Models;
using FileStorage.Data.UnitOfWork;
using FileStorage.Data.QueryModels;

namespace FileStorage.Domain.Services.PrivateItems
{
    public class PrivateItemsService : IPrivateItemsService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IFolderManager folderManager;
        private readonly IFileManager fileManager;
        private readonly string targetPath;
        private readonly long fileSizeLimit;

        public PrivateItemsService(IUnitOfWork unitOfWork,
                                   IMapper mapper,
                                   IFolderManager folderManager,
                                   IFileManager fileManager,
                                   IConfiguration configuration)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.folderManager = folderManager;
            this.fileManager = fileManager;
            targetPath = configuration.GetValue<string>("StoredFilesPath");
            fileSizeLimit = configuration.GetValue<long>("FileSizeLimit");
        }

        #region Files Operations

        public async Task<(IEnumerable<FileItemDto> files, PaginationHeader header)> GetPrivateFilesAndHeaderAsync(
            UserDto userDto, StorageItemsRequestParameters itemsParams)
        {
            var user = mapper.Map<UserDto, User>(userDto);
            var parameters = mapper.Map<StorageItemsRequest>(itemsParams);

            var (files, totalCount) = 
                await unitOfWork.StorageItems.GetPrivateFilesByUserAsync(user, parameters);

            var pagingHeader = PagingManager.PrepareHeader(totalCount, itemsParams);
            var filesDto = mapper.Map<IEnumerable<StorageItem>, IEnumerable<FileItemDto>>(files);

            return (filesDto, pagingHeader);
        }

        public async Task<FileItemDto> CreateFileAsync(UserDto userDto, IFormFile file)
        {
            var user = mapper.Map<UserDto, User>(userDto);
            var primaryFolder = await unitOfWork.StorageItems.GetUserPrimaryFolderAsync(user);

            if (primaryFolder == null)
            {
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

                    var fileDto = mapper.Map<StorageItem, FileItemDto>(fileItem);
                    return fileDto;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public async Task<(MemoryStream stream, string contentType, string fileName)> DownloadFileAsync(
            UserDto userDto, string fileId)
        {
            var fileItem = await GetActualItemByUserAndItemIdAsync(userDto, fileId);

            var filePath = StorageItemsHelpers.GetStorageItemFullPath(targetPath, fileItem.RelativePath);

            var stream = await fileManager.ReadFileAsync(filePath);
            stream.Position = 0;

            return (stream, StorageItemsHelpers.GetContentType(filePath), fileItem.DisplayName);
        }

        public async Task<(MemoryStream stream, string contentType, string fileName)> DownloadFileAsync(
            string fileId)
        {
            if (Guid.TryParse(fileId, out Guid storageItemId) == false)
                throw new ArgumentException($"{fileId} is not valid id");

            var file = await unitOfWork.StorageItems.GetFileByFileIdAsync(storageItemId);

            if (file == null)
                throw new StorageItemNotFoundException($"File for current user does not exist.");

            var filePath = StorageItemsHelpers.GetStorageItemFullPath(targetPath, file.RelativePath);

            var stream = await fileManager.ReadFileAsync(filePath);
            stream.Position = 0;

            return (stream, StorageItemsHelpers.GetContentType(filePath), file.DisplayName);
        }

        public async Task MoveFileRecycleBinAsync(UserDto userDto, string fileId)
        {
            var fileItem = await GetActualItemByUserAndItemIdAsync(userDto, fileId);
            
            fileItem.IsRecycled = true;

            await unitOfWork.CommitAsync();
        }

        public async Task MoveFilePublicAsync(UserDto userDto, string fileId)
        {
            var fileItem = await GetActualItemByUserAndItemIdAsync(userDto, fileId);

            fileItem.IsPublic = true;

            await unitOfWork.CommitAsync();
        }

        #endregion

        #region Folders Operations
        public async Task<StorageItem> CreateFolderAsync(FolderCreateRequestDto folderCreate)
        {
            var folder = mapper.Map<FolderCreateRequestDto, StorageItem>(folderCreate);

            folder.TrustedName = StorageItemsHelpers.GetStorageItemTrustedName();
            folder.RelativePath = StorageItemsHelpers.GetFolderRelativePath(folder);

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
            var user = mapper.Map<UserDto, User>(userDto);

            if (Guid.TryParse(id, out Guid storageItemId) == false)
                throw new ArgumentException($"{id} is not valid id");

            var folderToDelete =
                await unitOfWork.StorageItems.GetFolderByUserAndFolderIdAsync(user, storageItemId);

            if (folderToDelete == null)
                throw new ArgumentException("Folder doesn't exists");

            string fullPath = StorageItemsHelpers.GetStorageItemFullPath(targetPath, folderToDelete.RelativePath);

            if (!folderManager.IsFolderExists(fullPath))
                folderManager.DeleteFolder(fullPath);

            unitOfWork.StorageItems.Remove(folderToDelete);
            await unitOfWork.CommitAsync();
        }

        #endregion

        private async Task<StorageItem> GetActualItemByUserAndItemIdAsync(UserDto userDto,
                                                                          string fileId)
        {
            var user = mapper.Map<UserDto, User>(userDto);

            if (Guid.TryParse(fileId, out Guid storageItemId) == false)
                throw new ArgumentException($"{fileId} is not valid id");

            var file = 
                await unitOfWork.StorageItems.GetPrivateFileByIdAsync(storageItemId);

            if (file == null)
                throw new StorageItemNotFoundException($"File does not exist.");

            if (file.UserId != user.Id)
                throw new ArgumentException($"{user.UserName} is not the current file owner");

            return file;
        }
    }
}
