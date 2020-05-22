using System.Collections.Generic;
using System.Threading.Tasks;
using FileStorage.Domain.PagingHelpers;
using FileStorage.Domain.RequestModels;
using FileStorage.Domain.DataTransferredObjects.StorageItemModels;

namespace FileStorage.Domain.Services.ItemsModeration
{
    public interface IItemsModerationService
    {
        Task<(IEnumerable<FileItemDto> files, PaginationHeader header)> GetFilesForModerationAndHeaderAsync(
            StorageItemsRequestParameters itemsParams);
        Task MoveFilePrivateAsync(string fileId);
        Task MoveFilePublicAsync(string fileId);
        Task MoveFileRecycleBinAsync(string fileId);
        Task RestoreRecycledFileAsync(string fileId);
        Task DeleteFileAsync(string fileId);
    }
}