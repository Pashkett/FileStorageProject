using System;
using Microsoft.EntityFrameworkCore;
using FileStorage.Data.Models;
using FileStorage.Data.Models.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace FileStorage.Data.Persistence
{
    public class FileStorageContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public FileStorageContext(DbContextOptions options)
            : base(options) { }

        public DbSet<StorageItem> StorageItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new StorageItemConfiguration());
        }
    }
}
