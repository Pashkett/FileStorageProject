using System.Linq;
using FileStorage.Data.Models;

namespace FileStorage.Data.Persistence.Extensions
{
    public static class RepositoryStorageItemExtensions
    {
        public static IQueryable<StorageItem> PageStorageItems(this IQueryable<StorageItem> storageItems, 
            int pageNumber, int pageSize)
        {
            return storageItems
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize);
        }
    }
}
