using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FileStorage.Data.Models.Configurations
{
    public class StorageItemConfiguration : IEntityTypeConfiguration<StorageItem>
    {
        public void Configure(EntityTypeBuilder<StorageItem> builder)
        {
            builder.HasKey(storageItem => storageItem.Id);

            builder.Property(storageItem => storageItem.TrustedName)
                .HasMaxLength(300)
                .IsRequired();

            builder.Property(storageItem => storageItem.DisplayName)
                .HasMaxLength(300)
                .IsRequired();

            builder.Property(storageItem => storageItem.IsRecycled)
                .HasDefaultValue(false);

            builder.Property(storageItem => storageItem.IsPublic)
                .HasDefaultValue(false);

            builder.Property(storageItem => storageItem.Extension)
                .HasMaxLength(20);

            builder.Property(storageItem => storageItem.RelativePath)
                .HasMaxLength(900)
                .IsRequired();

            builder.HasOne(storageItem => storageItem.User)
                .WithMany(user => user.StorageItems)
                .HasForeignKey(storageItem => storageItem.UserId);

            builder.HasOne(storageItem => storageItem.ParentFolder)
                .WithMany(user => user.StorageItems)
                .HasForeignKey(storageItem => storageItem.ParentFolderId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}