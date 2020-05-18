using System.Linq;
using FileStorage.Data.Models;

namespace FileStorage.Data.Persistence.Extensions
{
    public static class RepositoryStorageItemExtensions
    {
        public static IQueryable<StorageItem> PageStorageItems(
            this IQueryable<StorageItem> storageItems, 
            int pageNumber, 
            int pageSize)
        {
            return storageItems
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);
        }

        public static IQueryable<StorageItem> FilterStorageItemsBySize(
            this IQueryable<StorageItem> storageItems,
            long minSize,
            long maxSize)
        {
            return storageItems
                .Where(storageItem => storageItem.Size >= minSize
                                      && storageItem.Size <= maxSize);
        }
    }
}
