using System;
using System.Collections.Generic;
using System.Text;

namespace FileStorage.Data.Models
{
    public class StorageFolder
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public StringBuilder RelativePath { get; set; }
        public StorageFolder Parent { get; set; }
        public User User { get; set; }
        public virtual ICollection<StorageFolder> StorageFolders { get; set; }
        public virtual ICollection<StorageFile> StorageFiles { get; set; }
    }
}
