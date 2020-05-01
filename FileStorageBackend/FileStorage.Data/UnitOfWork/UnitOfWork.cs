using FileStorage.Data.Persistence;
using FileStorage.Data.Persistence.Interfaces;
using FileStorage.Data.Persistence.Repositories;
using System.Threading.Tasks;

namespace FileStorage.Data.UnitOfWork
{
    public class EfUnitOfWork : IUnitOfWork
    {
        private readonly FileStorageContext context;

        private IStorageItemRepository storageItemRepository;
        private IRolesRepository rolesRepository;

        public EfUnitOfWork(FileStorageContext context)
        {
            this.context = context;
        }

        public IStorageItemRepository StorageItems => 
            storageItemRepository ?? (storageItemRepository = new StorageItemRepository(context));

        public IRolesRepository Roles =>
            rolesRepository ?? (rolesRepository = new RolesRepository(context));


        public async Task<int> CommitAsync()
        {
            return await context.SaveChangesAsync();
        }

        public void Dispose() => context.Dispose();
    }
}
