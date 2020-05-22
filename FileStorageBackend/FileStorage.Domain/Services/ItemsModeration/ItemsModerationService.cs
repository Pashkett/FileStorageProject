using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using FileStorage.Domain.DataTransferredObjects.StorageItemModels;
using FileStorage.Domain.Exceptions;
using FileStorage.Domain.PagingHelpers;
using FileStorage.Domain.RequestModels;
using FileStorage.Domain.Utilities;
using FileStorage.Data.FileSystemManagers.StorageFileManager;
using FileStorage.Data.Models;
using FileStorage.Data.QueryModels;
using FileStorage.Data.UnitOfWork;

namespace FileStorage.Domain.Services.ItemsModeration
{
    public class ItemsModerationService : IItemsModerationService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IFileManager fileManager;
        private readonly string targetPath;

        public ItemsModerationService(IUnitOfWork unitOfWork,
                                      IMapper mapper,
                                      IFileManager fileManager,
                                      IConfiguration configuration)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.fileManager = fileManager;
            targetPath = configuration.GetValue<string>("StoredFilesPath");
        }

        public async Task<(IEnumerable<FileItemDto> files, PaginationHeader header)> GetFilesForModerationAndHeaderAsync(
            StorageItemsRequestParameters itemsParams)
        {
            var parameters = mapper.Map<StorageItemsRequest>(itemsParams);

            var (files, totalCount) =
                await unitOfWork.StorageItems.GetAllFilesAsync(parameters);

            var pagingHeader = PagingManager.PrepareHeader(totalCount, itemsParams);
            var filesDto = mapper.Map<IEnumerable<StorageItem>, IEnumerable<FileItemDto>>(files);

            return (filesDto, pagingHeader);
        }

        public async Task MoveFileRecycleBinAsync(string fileId)
        {
            var fileItem = await GetActualItemByIdAsync(fileId);

            fileItem.IsRecycled = true;

            await unitOfWork.CommitAsync();
        }

        public async Task RestoreRecycledFileAsync(string fileId)
        {
            var fileItem = await GetActualItemByIdAsync(fileId);

            fileItem.IsRecycled = false;

            await unitOfWork.CommitAsync();
        }

        public async Task MoveFilePublicAsync(string fileId)
        {
            var fileItem = await GetActualItemByIdAsync(fileId);

            fileItem.IsPublic = true;

            await unitOfWork.CommitAsync();
        }

        public async Task MoveFilePrivateAsync(string fileId)
        {
            var fileItem = await GetActualItemByIdAsync(fileId);

            fileItem.IsPublic = false;

            await unitOfWork.CommitAsync();
        }

        public async Task DeleteFileAsync(string fileId)
        {
            var file = await GetActualItemByIdAsync(fileId);

            var filePath = StorageItemsHelpers.GetStorageItemFullPath(targetPath, file.RelativePath);
            fileManager.DeleteFile(filePath);

            unitOfWork.StorageItems.Remove(file);
            await unitOfWork.CommitAsync();
        }

        private async Task<StorageItem> GetActualItemByIdAsync(string fileId)
        {
            if (Guid.TryParse(fileId, out Guid storageItemId) == false)
                throw new ArgumentException($"{fileId} is not valid id");

            var file = await unitOfWork.StorageItems.GetFileByFileIdAsync(storageItemId);

            if (file == null)
                throw new StorageItemNotFoundException($"File for current user does not exist.");

            return file;
        }
    }
}
