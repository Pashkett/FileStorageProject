﻿using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using FileStorage.Domain.DataTransferredObjects.StorageItemModels;
using FileStorage.Domain.DataTransferredObjects.UserModels;
using FileStorage.Domain.PagingHelpers;
using FileStorage.Domain.RequestModels;
using FileStorage.Data.Models;

namespace FileStorage.Domain.Services.ActualItemsServices
{
    public interface IActualItemsService
    {
        Task<(IEnumerable<FileItemDto> pagedList, PaginationHeader paginationHeader)> 
            GetActualFilesByUserPagedAsync(UserDto userDto, StorageItemsRequestParameters itemsParams);
        Task<FileItemDto> CreateFileAsync(UserDto userDto, IFormFile file);
        Task<(MemoryStream stream, string contentType, string fileName)> DownloadFileAsync(
            UserDto userDto, string fileId);
        Task<(MemoryStream stream, string contentType, string fileName)> DownloadFileAsync(
            string fileId);
        Task MoveFileRecycledBinAsync(UserDto userDto, string fileId);
        Task MoveFilePublicAsync(UserDto userDto, string fileId);
        Task<StorageItem> CreateFolderAsync(FolderCreateRequestDto folderCreate);
        Task DeleteFolderAsync(UserDto userDto, string id);
    }
}