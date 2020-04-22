using System;
using System.Collections.Generic;
using System.Text;

namespace FileStorage.Data.Models
{
    public class StorageFile
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public User User { get; set; }
        public StorageFolder StorageFolder { get; set; }
    }
}
