using System;
using System.Threading.Tasks;
using FileStorage.Data.Persistence.Repositories.Interfaces;

namespace FileStorage.Data.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IStorageItemRepository StorageItems { get; }
        IUserRepository Users { get; }
        Task<int> CommitAsync();
    }
}
