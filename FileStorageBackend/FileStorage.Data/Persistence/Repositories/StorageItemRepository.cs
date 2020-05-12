using FileStorage.Data.Models;
using FileStorage.Data.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileStorage.Data.Persistence.Repositories
{
    public class StorageItemRepository : RepositoryBase<StorageItem>, IStorageItemRepository
    {
        private FileStorageContext fileStorageContext =>
            context as FileStorageContext;

        public StorageItemRepository(DbContext dbContext)
            : base(dbContext) { }

        public async Task<IEnumerable<StorageItem>> GetAllFilesByUserAsync(User user)
        {
            return await fileStorageContext.StorageItems
                                .Where(file => file.User == user
                                                  && file.IsFolder == false
                                                  && file.IsRecycled == false).ToListAsync();
        }

        public async Task<StorageItem> GetFileByFileIdAsync(Guid fileId)
        {
            return await fileStorageContext.StorageItems
                                .FirstOrDefaultAsync(file => file.Id == fileId
                                                                && file.IsFolder == false
                                                                && file.IsRecycled == false);
        }

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

        public async Task<IEnumerable<StorageItem>> GetAllRecycledFilesByUserAsync(User user)
        {
            return await fileStorageContext.StorageItems
                                .Where(file => file.User == user
                                                  && file.IsFolder == false
                                                  && file.IsRecycled == true).ToListAsync();
        }

        public async Task<StorageItem> GetRecycledFileByUserAndFileIdAsync(User user, Guid fileId)
        {
            return await fileStorageContext.StorageItems
                                .FirstOrDefaultAsync(file => file.Id == fileId
                                                                && file.User == user
                                                                && file.IsFolder == false
                                                                && file.IsRecycled == true);
        }

        public async Task<IEnumerable<StorageItem>> GetAllPublicFilesAsync()
        {
            return await fileStorageContext.StorageItems
                                .Where(file => file.IsPublic == true
                                                  && file.IsFolder == false
                                                  && file.IsRecycled == false).ToListAsync();
        }

        public async Task<StorageItem> GetPublicFileByUserAndFileIdAsync(User user, Guid fileId)
        {
            return await fileStorageContext.StorageItems
                                .FirstOrDefaultAsync(file => file.Id == fileId
                                                                && file.User == user
                                                                && file.IsFolder == false
                                                                && file.IsRecycled == false
                                                                && file.IsPublic == true);
        }
    }
}
