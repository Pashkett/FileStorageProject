using System;

namespace FileStorage.Data.Models
{
    public class StorageItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsFolder { get; set; }
        public bool IsRootFolder { get; set; }
        public string RelativePath { get; set; } 
        public string RootFolderName { get; set; }
        public User User { get; set; }
    }
}
