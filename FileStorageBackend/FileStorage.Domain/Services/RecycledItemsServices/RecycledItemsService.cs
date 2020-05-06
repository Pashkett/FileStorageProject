using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using FileStorage.Data.FileSystemManagers.StorageFileManager;
using FileStorage.Data.Models;
using FileStorage.Data.UnitOfWork;
using FileStorage.Domain.DataTransferredObjects.StorageItemModels;
using FileStorage.Domain.DataTransferredObjects.UserModels;
using FileStorage.Domain.Exceptions;
using FileStorage.Domain.Utilities;


namespace FileStorage.Domain.Services.RecycledItemsServices
{
    public class RecycledItemsService : IRecycledItemsService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IFileManager fileManager;
        private readonly string targetPath;

        public RecycledItemsService(IUnitOfWork unitOfWork,
                                    IMapper mapper,
                                    IFileManager fileManager,
                                    IConfiguration configuration)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.fileManager = fileManager;
            targetPath = configuration.GetValue<string>("StoredFilesPath");
        }

        public async Task<IEnumerable<FileItemDto>> GetRecycledItemsByUserAsync(UserDto userDto)
        {
            var user = mapper.Map<UserDto, User>(userDto);

            var files = await unitOfWork.StorageItems.GetAllRecycledFilesByUserAsync(user);

            var filesDto = mapper.Map<IEnumerable<StorageItem>, IEnumerable<FileItemDto>>(files);

            return filesDto;
        }

        public async Task DeleteRecycledItemAsync(UserDto userDto, string fileId)
        {
            var file = await GetRecycledItemByUserAndItemIdAsync(userDto, fileId);

            var filePath = StorageItemsHelpers.GetStorageItemFullPath(targetPath, file.RelativePath);
            fileManager.DeleteFile(filePath);

            unitOfWork.StorageItems.Remove(file);
            await unitOfWork.CommitAsync();

        }

        public async Task RestoreRecycledItemAsync(UserDto userDto, string fileId)
        {
            var file = await GetRecycledItemByUserAndItemIdAsync(userDto, fileId);

            file.IsRecycled = false;
            await unitOfWork.CommitAsync();
        }

        private async Task<StorageItem> GetRecycledItemByUserAndItemIdAsync(UserDto userDto, string fileId)
        {
            var user = mapper.Map<UserDto, User>(userDto);

            if (Guid.TryParse(fileId, out Guid storageItemId) == false)
                throw new ArgumentException($"{fileId} is not valid id");

            var file = await unitOfWork.StorageItems
                .GetRecycledFileByUserAndFileIdAsync(user, storageItemId);

            if (file == null)
                throw new StorageItemNotFoundException($"File for current user does not exist.");
            return file;
        }
    }
}
