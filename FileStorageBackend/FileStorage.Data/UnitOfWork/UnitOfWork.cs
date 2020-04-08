using System;
using FileStorage.Persistence;
using FileStorage.Persistence.Repositories;
using FileStorage.Persistence.Repositories.Interfaces;

namespace FileStorage.UnitOfWork
{
    public class EfUnitOfWork : IUnitOfWork
    {
        private readonly FileStorageContext context;

        private IStorageItemRepository storageItemRepository;
        private IUserRepository userRepository;

        public EfUnitOfWork(FileStorageContext context)
        {
            this.context = context;
        }

        public IStorageItemRepository StorageItems =>
            storageItemRepository ?? (storageItemRepository = new StorageItemRepository(context));

        public IUserRepository Users =>
            userRepository ?? (userRepository = new UserRepository(context));

        public int Complete => context.SaveChanges();

        public void Dispose() => context.Dispose();
    }
}
