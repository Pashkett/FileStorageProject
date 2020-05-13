using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FileStorage.Data.Models;

namespace FileStorage.Data.Persistence.Interfaces
{
    public interface IStorageItemRepository : IRepositoryBase<StorageItem>
    {
        Task<IEnumerable<StorageItem>> GetAllFilesByUserAsync(User user);
        Task<StorageItem> GetFileByUserAndFileIdAsync(User user, Guid fileId);
        Task<StorageItem> GetFileByFileIdAsync(Guid fileId);
        Task<StorageItem> GetFolderByTrustedNameAsync(string trustedName);
        Task<StorageItem> GetUserPrimaryFolderAsync(User user);
        Task<StorageItem> GetFolderByUserAndFolderIdIdAsync(User user, Guid folderId);
        Task<IEnumerable<StorageItem>> GetAllRecycledFilesByUserAsync(User user);
        Task<StorageItem> GetRecycledFileByUserAndFileIdAsync(User user, Guid fileId);
        Task<IEnumerable<StorageItem>> GetAllPublicFilesAsync();
        Task<StorageItem> GetPublicFileByFileIdAsync(Guid fileId);
        Task<StorageItem> GetPublicFileByUserAndFileIdAsync(User user, Guid fileId);
    }
}