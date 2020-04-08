using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileStorage.Data.Models
{
    public class StorageItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsFolder { get; set; }
        public bool IsRootFolder { get; set; }
        public string RelativePath { get; set; } 
        public string RootFolder { get; set; }
        public User Owner { get; set; }
    }
}
