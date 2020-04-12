using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FileStorage.Data.Models.Configurations
{
    class StorageItemConfiguration : IEntityTypeConfiguration<StorageItem>
    {
        public void Configure(EntityTypeBuilder<StorageItem> builder)
        {
            builder.HasKey(storageItem => storageItem.Id);

            builder.HasOne(storageItem => storageItem.User)
                .WithMany(user => user.StorageItems);

            builder.Property(storageItem => storageItem.Name)
                .HasMaxLength(1000)
                .IsRequired();

            builder.Property(storageItem => storageItem.RelativePath)
                .HasMaxLength(1000)
                .IsRequired();

            builder.Property(storageItem => storageItem.RootFolderName)
                .HasMaxLength(1000)
                .IsRequired();

            builder.Property(storageItem => storageItem.IsRootFolder)
                .IsRequired();

            builder.Property(storageItem => storageItem.IsFolder)
                .IsRequired();
        }
    }
}