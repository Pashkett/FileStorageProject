using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using FileStorage.Data.Models;
using FileStorage.Domain.DataTransferredObjects.StorageItemModels;
using FileStorage.Domain.DataTransferredObjects.UserModels;
using System.IO;

namespace FileStorage.Domain.Services.StorageItemServices
{
    public interface IStorageItemsService
    {
        Task CreateFileAsync(UserDto user, IFormFile file);
        Task<(MemoryStream stream, string contentType, string fileName)> DownloadFileAsync(UserDto userDto, string fileId);

        //Task RenameFileAsync(string id, string folderNewName);
        //Task MoveFileToRecycledBinAsync(string id, string folderNewName);
        //Task DeleteFileAsync(UserDto userDto, string id);

        Task<StorageItem> CreateFolderAsync(FolderCreateRequestDto folderCreate);
        Task DeleteFolderAsync(UserDto userDto, string id);
    }
}