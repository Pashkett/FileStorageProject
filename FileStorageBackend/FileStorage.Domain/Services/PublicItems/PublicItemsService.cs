using AutoMapper;
using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using FileStorage.Domain.DataTransferredObjects.StorageItemModels;
using FileStorage.Domain.DataTransferredObjects.UserModels;
using FileStorage.Domain.Exceptions;
using FileStorage.Domain.Utilities;
using FileStorage.Domain.PagingHelpers;
using FileStorage.Domain.RequestModels;
using FileStorage.Data.Models;
using FileStorage.Data.UnitOfWork;
using FileStorage.Data.FileSystemManagers.StorageFileManager;
using FileStorage.Data.QueryModels;


namespace FileStorage.Domain.Services.PublicItems
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

        public async Task<(IEnumerable<FileItemDto> files, PaginationHeader header)> GetPublicFilesAndHeaderAsync(
            StorageItemsRequestParameters itemsParams)
        {
            var parameters = mapper.Map<StorageItemsRequest>(itemsParams);

            var (files, totalCount) = 
                await unitOfWork.StorageItems.GetPublicFilesAsync(parameters);

            var pagingHeader = PagingManager.PrepareHeader(totalCount, itemsParams);

            var filesDto = mapper.Map<IEnumerable<StorageItem>, IEnumerable<FileItemDto>>(files);

            return (filesDto, pagingHeader);
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

            var fileItem = await unitOfWork.StorageItems.GetPublicFileByIdAsync(storageItemId);

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

            var file = 
                await unitOfWork.StorageItems.GetPublicFileByIdAsync(storageItemId);

            if (file == null)
                throw new StorageItemNotFoundException($"File does not exist.");

            if (file.UserId != user.Id)
                throw new ArgumentException($"{user.UserName} is not the current file owner");

            return file;
        }
    }
}
