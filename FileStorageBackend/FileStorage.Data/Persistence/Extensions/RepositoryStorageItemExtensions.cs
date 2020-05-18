using System.Linq;
using System.Linq.Dynamic.Core;
using FileStorage.Data.Models;

namespace FileStorage.Data.Persistence.Extensions
{
    public static class RepositoryStorageItemExtensions
    {
        public static IQueryable<StorageItem> PageStorageItems(
            this IQueryable<StorageItem> storageItems, int pageNumber, int pageSize)
        {
            return storageItems
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);
        }

        public static IQueryable<StorageItem> FilterStorageItemsBySize(
            this IQueryable<StorageItem> storageItems, long minSize, long maxSize)
        {
            return storageItems.Where(storageItem => 
                storageItem.Size >= minSize
                && storageItem.Size <= maxSize);
        }

        public static IQueryable<StorageItem> SearchBy(
            this IQueryable<StorageItem> storageItems, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return storageItems;

            var lowerCaseTerm = searchTerm.Trim().ToLower();

            return storageItems.Where(storageItem => 
                storageItem.DisplayName.ToLower().Contains(lowerCaseTerm));
        }

        public static IQueryable<StorageItem> Sort(
            this IQueryable<StorageItem> storageItems, string orderByString)
        {
            if (string.IsNullOrWhiteSpace(orderByString))
                return storageItems.OrderBy(storageItem => storageItem.DisplayName);

            string orderQuery = orderByString.CreateOrderQuery<StorageItem>();

            if (string.IsNullOrWhiteSpace(orderQuery))
                return storageItems.OrderBy(storageItem => storageItem.DisplayName);

            return storageItems.OrderBy(orderQuery);
        }
    }
}
