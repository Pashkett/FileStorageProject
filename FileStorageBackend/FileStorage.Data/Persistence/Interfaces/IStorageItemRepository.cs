using System;
using System.Threading.Tasks;
using FileStorage.Data.Models;

namespace FileStorage.Data.Persistence.Interfaces
{
    public interface IStorageItemRepository : IRepositoryBase<StorageItem>
    {
        Task<StorageItem> GetFileByUserAndFileIdAsync(User user, Guid fileId);
        Task<StorageItem> GetFolderByTrustedNameAsync(string trustedName);
        Task<StorageItem> GetUserPrimaryFolderAsync(User user);
        Task<StorageItem> GetFolderByUserAndFolderIdIdAsync(User user, Guid folderId);
    }
}