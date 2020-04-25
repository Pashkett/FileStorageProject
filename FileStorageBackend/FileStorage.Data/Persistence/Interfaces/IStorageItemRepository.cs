using FileStorage.Data.Models;
using System.Threading.Tasks;

namespace FileStorage.Data.Persistence.Interfaces
{
    public interface IStorageItemRepository : IRepositoryBase<StorageItem>
    {
        Task<StorageItem> FindByTrustedNameAsync(string trustedName);
    }
}