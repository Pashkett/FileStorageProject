using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FileStorage.Data.Models.Configurations
{
    class StorageFileConfiguration : IEntityTypeConfiguration<StorageFile>
    {
        public void Configure(EntityTypeBuilder<StorageFile> builder)
        {
            builder.HasKey(storageFile => storageFile.Id);

            builder.Property(storageFile => storageFile.Name)
                .HasMaxLength(300)
                .IsRequired();

            builder.HasOne(storageFile => storageFile.User)
                .WithMany(user => user.StorageFiles)
                .IsRequired();

            builder.HasOne(storageFile => storageFile.StorageFolder)
                .WithMany(storageFolder => storageFolder.StorageFiles)
                .IsRequired();
        }
    }
}
