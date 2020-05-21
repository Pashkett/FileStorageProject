using AutoMapper;
using FileStorage.Data.FileSystemManagers.StorageFileManager;
using FileStorage.Data.Models;
using FileStorage.Data.QueryModels;
using FileStorage.Data.UnitOfWork;
using FileStorage.Domain.DataTransferredObjects.StorageItemModels;
using FileStorage.Domain.Exceptions;
using FileStorage.Domain.PagingHelpers;
using FileStorage.Domain.RequestModels;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FileStorage.Domain.Services.ItemsModerationServices
{
    public class ItemsModerationService
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

        public async Task<(IEnumerable<FileItemDto> pagedList, PaginationHeader paginationHeader)>
            GetFilesModerationPagedAsync(StorageItemsRequestParameters itemsParams)
        {
            var parameters = mapper.Map<StorageItemsRequest>(itemsParams);

            var (files, totalCount) =
                await unitOfWork.StorageItems.GetAllFilesAsync(parameters);

            var pagingHeader = PagingManager.PrepareHeader(totalCount, itemsParams);
            var filesDto = mapper.Map<IEnumerable<StorageItem>, IEnumerable<FileItemDto>>(files);

            return (filesDto, pagingHeader);
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
