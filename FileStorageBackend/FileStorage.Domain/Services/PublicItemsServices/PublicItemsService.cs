using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FileStorage.Data.Models;
using FileStorage.Data.UnitOfWork;
using FileStorage.Domain.DataTransferredObjects.StorageItemModels;
using FileStorage.Domain.DataTransferredObjects.UserModels;
using FileStorage.Domain.Exceptions;
using System.IO;
using FileStorage.Domain.Utilities;
using FileStorage.Data.FileSystemManagers.StorageFileManager;
using Microsoft.Extensions.Configuration;
using FileStorage.Domain.PagingHelpers;

namespace FileStorage.Domain.Services.PublicItemsServices
{
    public class PublicItemsService : IPublicItemsService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IFileManager fileManager;
        private readonly string targetPath;

        public PublicItemsService(IUnitOfWork unitOfWork,
                                  IMapper mapper,
                                  IFileManager fileManager,
                                  IConfiguration configuration)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.fileManager = fileManager;
            targetPath = configuration.GetValue<string>("StoredFilesPath");
        }

        public async Task<IEnumerable<FileItemDto>> GetPublicFilesAsync()
        {
            var files = await unitOfWork.StorageItems.GetAllPublicFilesAsync();

            var filesDto = mapper.Map<IEnumerable<StorageItem>, IEnumerable<FileItemDto>>(files);

            return filesDto;
        }

        public async Task<(IEnumerable<FileItemDto> pagedList, PaginationHeader paginationHeader)> 
            GetPublicFilesPagedAsync(StorageItemsRequestParameters itemsParams)
        {
            var files = await unitOfWork.StorageItems.GetAllPublicFilesAsync();

            var pagingResult = PagingManager<StorageItem>.ProcessPaging(
                files,
                itemsParams.PageNumber,
                itemsParams.PageSize);

            var filesDto = mapper.Map<IEnumerable<StorageItem>, IEnumerable<FileItemDto>>(
                pagingResult.resultedCollection);

            return (filesDto, pagingResult.paginationHeader);
        }

        public async Task MoveFilePrivateAsync(UserDto userDto, string fileId)
        {
            var fileItem = await GetPublicItemByUserAndItemIdAsync(userDto, fileId);

            fileItem.IsPublic = false;

            await unitOfWork.CommitAsync();
        }

        public async Task<(MemoryStream stream, string contentType, string fileName)> DownloadFileAsync(string fileId)
        {
            if (Guid.TryParse(fileId, out Guid storageItemId) == false)
                throw new ArgumentException($"{fileId} is not valid id");

            var fileItem = await unitOfWork.StorageItems.GetPublicFileByFileIdAsync(storageItemId);

            if (fileItem == null)
                throw new StorageItemNotFoundException($"File for current user does not exist.");

            var filePath = StorageItemsHelpers.GetStorageItemFullPath(targetPath, fileItem.RelativePath);

            var stream = await fileManager.ReadFileAsync(filePath);
            stream.Position = 0;

            return (stream, StorageItemsHelpers.GetContentType(filePath), fileItem.DisplayName);

        }

        private async Task<StorageItem> GetPublicItemByUserAndItemIdAsync(UserDto userDto, string fileId)
        {
            var user = mapper.Map<UserDto, User>(userDto);

            if (Guid.TryParse(fileId, out Guid storageItemId) == false)
                throw new ArgumentException($"{fileId} is not valid id");

            var file = await unitOfWork.StorageItems
                .GetPublicFileByUserAndFileIdAsync(user, storageItemId);

            if (file == null)
                throw new StorageItemNotFoundException($"File for current user does not exist.");
            
            return file;
        }
    }
}
