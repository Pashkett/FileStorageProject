using FileStorage.Data.Models;
using FileStorage.Data.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace FileStorage.Data.Persistence.Repositories
{
    public class StorageItemRepository : RepositoryBase<StorageItem>, IStorageItemRepository
    {
        private FileStorageContext fileStorageContext => 
            context as FileStorageContext;

        public StorageItemRepository(DbContext dbContext) 
            : base(dbContext) { }

        public async Task<StorageItem> GetFileByUserAndFileIdAsync(User user, Guid fileId)
        {
            return await fileStorageContext.StorageItems
                                .FirstOrDefaultAsync(file => file.Id == fileId
                                                                && file.User == user
                                                                && file.IsFolder == false
                                                                && file.IsRecycled == false);
        }

        public async Task<StorageItem> GetFolderByTrustedNameAsync(string trustedName)
        {
            return await fileStorageContext.StorageItems
                                .FirstOrDefaultAsync(folder => folder.TrustedName == trustedName
                                                                && folder.IsFolder == true
                                                                && folder.IsRecycled == false);
        }

        public async Task<StorageItem> GetUserPrimaryFolderAsync(User user)
        {
            return await fileStorageContext.StorageItems
                                .FirstOrDefaultAsync(folder => folder.IsPrimaryFolder == true
                                                                && folder.User == user
                                                                && folder.IsRecycled == false);
        }

        public async Task<StorageItem> GetFolderByUserAndFolderIdIdAsync(User user, Guid folderId)
        {
            return await fileStorageContext.StorageItems
                                .FirstOrDefaultAsync(folder => folder.Id == folderId
                                                                && folder.User == user
                                                                && folder.IsFolder == true
                                                                && folder.IsRecycled == false);
        }
    }
}
