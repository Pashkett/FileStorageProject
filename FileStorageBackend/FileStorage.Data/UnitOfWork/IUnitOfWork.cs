using System;
using FileStorage.Data.Persistence.Repositories.Interfaces;

namespace FileStorage.Data.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IStorageItemRepository StorageItems { get; }
        IUserRepository Users { get; }
        int Complete { get; }
    }
}
