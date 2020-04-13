using FileStorage.Data.Models;
using FileStorage.Data.Persistence.Repositories.Interfaces;

namespace FileStorage.Data.Persistence.Repositories
{
    public class StorageItemRepository : RepositoryBase<StorageItem>, IStorageItemRepository
    {
        private FileStorageContext fileStorageContext
        {
            get => context as FileStorageContext;
        }

        public StorageItemRepository(FileStorageContext context) : base(context) { }
    }
}
