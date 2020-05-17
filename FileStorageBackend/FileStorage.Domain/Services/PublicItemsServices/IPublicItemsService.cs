using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using FileStorage.Domain.DataTransferredObjects.StorageItemModels;
using FileStorage.Domain.DataTransferredObjects.UserModels;
using FileStorage.Domain.PagingHelpers;
using FileStorage.Domain.RequestModels;


namespace FileStorage.Domain.Services.PublicItemsServices
{
    public interface IPublicItemsService
    {
        Task<(IEnumerable<FileItemDto> pagedList, PaginationHeader paginationHeader)>
            GetPublicFilesPagedAsync(StorageItemsRequestParameters itemsParams);
        Task MoveFilePrivateAsync(UserDto userDto, string fileId);
        Task<(MemoryStream stream, string contentType, string fileName)> DownloadFileAsync(string fileId);
    }
}