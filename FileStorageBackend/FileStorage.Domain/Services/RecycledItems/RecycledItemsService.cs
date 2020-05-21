using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using FileStorage.Domain.DataTransferredObjects.StorageItemModels;
using FileStorage.Domain.DataTransferredObjects.UserModels;
using FileStorage.Domain.Exceptions;
using FileStorage.Domain.Utilities;
using FileStorage.Domain.PagingHelpers;
using FileStorage.Domain.RequestModels;
using FileStorage.Data.QueryModels;
using FileStorage.Data.FileSystemManagers.StorageFileManager;
using FileStorage.Data.Models;
using FileStorage.Data.UnitOfWork;


namespace FileStorage.Domain.Services.RecycledItems
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

        public async Task<(IEnumerable<FileItemDto> files, PaginationHeader header)> GetRecycledItemsAndHeaderAsync(
            UserDto userDto, StorageItemsRequestParameters itemsParams)
        {
            var user = mapper.Map<UserDto, User>(userDto);
            var parameters = mapper.Map<StorageItemsRequest>(itemsParams);

            var (files, totalCount) = 
                await unitOfWork.StorageItems.GetRecycledFilesByUserAsync(user, parameters);

            var pagingHeader = PagingManager.PrepareHeader(totalCount, itemsParams);
            var filesDto = mapper.Map<IEnumerable<StorageItem>, IEnumerable<FileItemDto>>(files);

            return (filesDto, pagingHeader);
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

            var file = 
                await unitOfWork.StorageItems.GetRecycledFileByIdAsync(storageItemId);

            if (file == null)
                throw new StorageItemNotFoundException($"File does not exist.");

            if (file.UserId != user.Id)
                throw new ArgumentException($"{user.UserName} is not the current file owner");

            return file;
        }
    }
}
