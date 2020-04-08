using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileStorage.Data.Models.Configurations
{
    class StorageItemConfiguration : IEntityTypeConfiguration<StorageItem>
    {
        public void Configure(EntityTypeBuilder<StorageItem> builder)
        {
            builder.HasKey(storageItem => storageItem.Id);

            builder.Property(storageItem => storageItem.Owner)
                .IsRequired();

            builder.Property(storageItem => storageItem.Name)
                .IsRequired();

            builder.Property(storageItem => storageItem.RelativePath)
                .IsRequired();

            builder.Property(storageItem => storageItem.IsRootFolder)
                .IsRequired();

            builder.Property(storageItem => storageItem.IsFolder)
                .IsRequired();
        }
    }
}
