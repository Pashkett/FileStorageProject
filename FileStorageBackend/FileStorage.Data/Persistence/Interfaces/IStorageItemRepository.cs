using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FileStorage.Data.Models;
using FileStorage.Data.QueryModels;

namespace FileStorage.Data.Persistence.Interfaces
{
    public interface IStorageItemRepository : IRepositoryBase<StorageItem>
    {
        Task<(IEnumerable<StorageItem> resultItems, int totalCount)>
            GetActualFilesByUserAsync(User user, StorageItemsRequest itemsRequest);
        Task<StorageItem> GetFileByUserAndFileIdAsync(User user, Guid fileId);
        Task<StorageItem> GetFileByFileIdAsync(Guid fileId);
        Task<StorageItem> GetFolderByTrustedNameAsync(string trustedName);
        Task<StorageItem> GetUserPrimaryFolderAsync(User user);
        Task<StorageItem> GetFolderByUserAndFolderIdIdAsync(User user, Guid folderId);
        Task<(IEnumerable<StorageItem> resultItems, int totalCount)>
            GetRecycledFilesByUserAsync(User user, StorageItemsRequest itemsRequest);
        Task<StorageItem> GetRecycledFileByUserAndFileIdAsync(User user, Guid fileId);
        Task<(IEnumerable<StorageItem> resultItems, int totalCount)>
            GetPublicFilesAsync(StorageItemsRequest itemsRequest);
        Task<StorageItem> GetPublicFileByFileIdAsync(Guid fileId);
        Task<StorageItem> GetPublicFileByUserAndFileIdAsync(User user, Guid fileId);
    }
}