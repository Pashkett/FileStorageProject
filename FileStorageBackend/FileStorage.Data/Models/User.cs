﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FileStorage.Data.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }  
        public virtual StorageItem UserRootFolder { get; set; }
        public virtual ICollection<StorageItem> StorageItems { get; set; }
    }
}
