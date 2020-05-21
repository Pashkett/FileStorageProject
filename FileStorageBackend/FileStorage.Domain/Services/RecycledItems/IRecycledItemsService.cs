using System.Collections.Generic;
using System.Threading.Tasks;
using FileStorage.Domain.DataTransferredObjects.StorageItemModels;
using FileStorage.Domain.DataTransferredObjects.UserModels;
using FileStorage.Domain.PagingHelpers;
using FileStorage.Domain.RequestModels;

namespace FileStorage.Domain.Services.RecycledItems
{
    public interface IRecycledItemsService
    {
        Task<(IEnumerable<FileItemDto> files, PaginationHeader header)> GetRecycledItemsAndHeaderAsync(
            UserDto userDto, StorageItemsRequestParameters itemsParams);
        Task RestoreRecycledItemAsync(UserDto userDto, string fileId);
        Task DeleteRecycledItemAsync(UserDto userDto, string fileId);
    }
}