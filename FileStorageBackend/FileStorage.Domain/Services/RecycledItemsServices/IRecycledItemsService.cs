﻿using System.Collections.Generic;
using System.Threading.Tasks;
using FileStorage.Domain.DataTransferredObjects.StorageItemModels;
using FileStorage.Domain.DataTransferredObjects.UserModels;
using FileStorage.Domain.PagingHelpers;

namespace FileStorage.Domain.Services.RecycledItemsServices
{
    public interface IRecycledItemsService
    {
        Task<IEnumerable<FileItemDto>> GetRecycledItemsByUserAsync(UserDto userDto);
        Task<(IEnumerable<FileItemDto> pagedList, PaginationHeader paginationHeader)>
            GetRecycledItemsByUserPagedAsync(UserDto userDto, StorageItemsRequestParameters itemsParams);
        Task RestoreRecycledItemAsync(UserDto userDto, string fileId);
        Task DeleteRecycledItemAsync(UserDto userDto, string fileId);
    }
}