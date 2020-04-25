using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FileStorage.Data.Models.Configurations
{
    public class StorageItemConfiguration : IEntityTypeConfiguration<StorageItem>
    {
        public void Configure(EntityTypeBuilder<StorageItem> builder)
        {
            builder.HasKey(storageFolder => storageFolder.Id);

            builder.Property(storageFolder => storageFolder.TrustedName)
                .HasMaxLength(300)
                .IsRequired();

            builder.Property(storageFolder => storageFolder.DisplayName)
                .HasMaxLength(300)
                .IsRequired();

            builder.Property(storageFolder => storageFolder.RelativePath)
                .HasMaxLength(900)
                .IsRequired();

            builder.HasOne(storageFolder => storageFolder.User)
                .WithMany(user => user.StorageItems);
        }
    }
}