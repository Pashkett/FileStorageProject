using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using AutoMapper;
using FileStorage.Data.FileSystemManagers.StorageFolderManager;
using FileStorage.Data.Models;
using FileStorage.Data.UnitOfWork;
using FileStorage.Domain.DataTransferredObjects.StorageItemModels;

namespace FileStorage.Domain.Services.FolderServices
{
    public class FolderService : IFolderService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IFolderManager folderManager;
        private readonly string targetPath;

        public FolderService(IUnitOfWork unitOfWork, IMapper mapper, IFolderManager folderManager, 
                            IConfiguration configuration)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.folderManager = folderManager;
            targetPath = configuration.GetValue<string>("StoredFilesPath");
        }

        public async Task<IEnumerable<StorageItemDto>> GetAllStorageItemsAsync()
        {
            var storageItems = await unitOfWork.StorageItems.GetAllAsync();
            var storageItemsDto = mapper.Map<IEnumerable<StorageItem>, IEnumerable<StorageItemDto>>(storageItems);

            return storageItemsDto;
        }

        public async Task<StorageItem> CreateFolderAsync(FolderCreateRequestDto folderCreate)
        {
            var folder = mapper.Map<FolderCreateRequestDto, StorageItem>(folderCreate);

            folder.TrustedName = GetFolderTrustedName(folder);
            folder.RelativePath = GetFolderRelativePath(folder);

            var existingFolderInDirectory = 
                await unitOfWork.StorageItems.FindByTrustedNameAsync(folder.TrustedName);

            string fullPath = FolderFullPathMaker(targetPath, folder.RelativePath);

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

        public async Task RenameFolderAsync(Guid id, string folderNewName)
        {
            var folderToUpdate = await unitOfWork.StorageItems.GetByIdAsync(id);

            if (folderToUpdate != null)
            {
                folderToUpdate.DisplayName = folderNewName;
                await unitOfWork.CommitAsync();
            }
            else
                throw new ArgumentException("Folder doesn't exists");
        }

        public async Task DeleteFolderAsync(Guid id)
        {
            var folderToDelete = await unitOfWork.StorageItems.GetByIdAsync(id);

            if (folderToDelete == null)
                throw new ArgumentException("Folder doesn't exists");

            string fullPath = FolderFullPathMaker(targetPath, folderToDelete.RelativePath);

            if (!folderManager.IsFolderExists(fullPath))
            {
                folderManager.DeleteFolder(fullPath);
            }

            unitOfWork.StorageItems.Remove(folderToDelete);
            await unitOfWork.CommitAsync();
        }

        private string GetFolderRelativePath(StorageItem folder)
        {
            if (folder.IsFolder)
            {
                if (folder.IsPrimaryFolder)
                    return folder.User.Id.ToString();

                else
                    return Path.Combine(folder.User.Id.ToString(), folder.TrustedName);
            }
            else
            {
                throw new ArgumentException("Record should be a folder");
            }
        }

        private string GetFolderTrustedName(StorageItem folder)
        {
            if (folder.IsFolder)
            {
                if (folder.IsPrimaryFolder == true)
                    return folder.User.Id.ToString();

                else
                    return $"{folder.User.Id}_{DateTime.Now:yyyyMMddTHHmmss.fff}";      
            }
            else
            {
                throw new ArgumentException("Record should be a folder");
            }
        }
    
        private string FolderFullPathMaker(string targetPath, string relativePath)
        {
            var parentPath = Directory.GetParent(Directory.GetCurrentDirectory()).ToString();

            if (targetPath == null || relativePath == null)
                throw new ArgumentNullException("One of the argument equals null");
            else
                return Path.Combine(parentPath, targetPath, relativePath);
        }
    }
}
