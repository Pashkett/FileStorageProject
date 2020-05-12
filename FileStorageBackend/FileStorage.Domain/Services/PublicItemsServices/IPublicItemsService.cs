using FileStorage.Domain.DataTransferredObjects.StorageItemModels;
using FileStorage.Domain.DataTransferredObjects.UserModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileStorage.Domain.Services.PublicItemsServices
{
    public interface IPublicItemsService
    {
        Task<IEnumerable<FileItemDto>> GetPublicFilesAsync();
        Task MoveFilePrivateAsync(UserDto userDto, string fileId);
    }
}