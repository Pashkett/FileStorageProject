using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using FileStorage.Data.Models;
using FileStorage.Domain.DataTransferredObjects.StorageItemModels;
using FileStorage.Domain.DataTransferredObjects.UserModels;


namespace FileStorage.Domain.Services.ActualItemsServices
{
    public interface IActualItemsService
    {
        Task<IEnumerable<FileItemDto>> GetActualFilesByUserAsync(UserDto userDto);
        Task CreateFileAsync(UserDto userDto, IFormFile file);
        Task<(MemoryStream stream, string contentType, string fileName)> DownloadFileAsync(
            UserDto userDto, string fileId);
        Task MoveFileToRecycledBinAsync(UserDto userDto, string fileId);

        //Task RenameFileAsync(string id, string itemNewName);
        //Task DeleteFileAsync(UserDto userDto, string id);

        Task<StorageItem> CreateFolderAsync(FolderCreateRequestDto folderCreate);
        Task DeleteFolderAsync(UserDto userDto, string id);
    }
}