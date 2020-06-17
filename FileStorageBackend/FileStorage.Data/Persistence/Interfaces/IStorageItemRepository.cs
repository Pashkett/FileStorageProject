using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FileStorage.Data.Models;
using FileStorage.Data.QueryModels;

namespace FileStorage.Data.Persistence.Interfaces
{
    public interface IStorageItemRepository : IRepositoryBase<StorageItem>
    {
        Task<(IEnumerable<StorageItem> files, int count)> GetPrivateFilesByUserAsync(
            User user, StorageItemsRequest itemsRequest);
        Task<StorageItem> GetPrivateFileByIdAsync(Guid fileId);
        Task<StorageItem> GetFolderByTrustedNameAsync(string trustedName);
        Task<StorageItem> GetUserPrimaryFolderAsync(User user);
        Task<StorageItem> GetFolderByUserAndFolderIdAsync(User user, Guid folderId);
        Task<(IEnumerable<StorageItem> files, int count)> GetRecycledFilesByUserAsync(
            User user, StorageItemsRequest itemsRequest);
        Task<StorageItem> GetRecycledFileByIdAsync(Guid fileId);
        Task<(IEnumerable<StorageItem> files, int count)> GetPublicFilesAsync(
            StorageItemsRequest itemsRequest);
        Task<StorageItem> GetPublicFileByIdAsync(Guid fileId);
        Task<(IEnumerable<StorageItem> files, int count)> GetAllFilesAsync(
            StorageItemsRequest itemsRequest);
        Task<StorageItem> GetFileByFileIdAsync(Guid fileId);
    }
}