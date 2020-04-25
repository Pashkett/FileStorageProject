using FileStorage.Data.Models;
using FileStorage.Domain.DataTransferedObjects.StorageItemModels;
using System.Threading.Tasks;

namespace FileStorage.Domain.Services.FolderServices
{
    public interface IFolderService
    {
        Task<StorageItem> CreateFolderAsync(FolderCreateRequestDto folderCreate);
    }
}