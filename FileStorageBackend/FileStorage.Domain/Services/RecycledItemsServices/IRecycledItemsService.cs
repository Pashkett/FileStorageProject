using FileStorage.Domain.DataTransferredObjects.StorageItemModels;
using FileStorage.Domain.DataTransferredObjects.UserModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileStorage.Domain.Services.RecycledItemsServices
{
    public interface IRecycledItemsService
    {
        Task DeleteRecycledItemAsync(UserDto userDto, string fileId);
        Task<IEnumerable<FileItemDto>> GetRecycledItemsByUserAsync(UserDto userDto);
        Task RestoreRecycledItemAsync(UserDto userDto, string fileId);
    }
}