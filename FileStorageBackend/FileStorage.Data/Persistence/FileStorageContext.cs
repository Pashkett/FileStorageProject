﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Microsoft.EntityFrameworkCore;
using FileStorage.Data.Models;
using FileStorage.Data.Models.Configurations;

namespace FileStorage.Persistence
{
    public class FileStorageContext : DbContext
    {
        public FileStorageContext(DbContextOptions options)
            : base(options) { }

        public DbSet<StorageItem> StorageItems { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new StorageItemConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
        }
    }
}
