using FileStorage.Data.Models;
using FileStorage.Data.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FileStorage.Data.Persistence.Repositories
{
    public class StorageItemRepository : RepositoryBase<StorageItem>, IStorageItemRepository
    {
        private FileStorageContext fileStorageContext => 
            context as FileStorageContext;

        public StorageItemRepository(DbContext dbContext) 
            : base(dbContext) { }

        public async Task<StorageItem> FindByTrustedNameAsync(string trustedName) =>
            await fileStorageContext.StorageItems
                    .FirstOrDefaultAsync(x => x.TrustedName == trustedName);
    }
}
