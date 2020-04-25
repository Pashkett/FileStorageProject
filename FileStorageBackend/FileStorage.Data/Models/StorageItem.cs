using System;
using System.Collections.Generic;

namespace FileStorage.Data.Models
{
    public class StorageItem
    {
        public Guid Id { get; set; }
        public string TrustedName { get; set; }
        public string DisplayName { get; set; }
        public bool IsFolder { get; set; }
        public bool? IsRootFolder { get; set; }
        public string RelativePath { get; set; }
        public virtual StorageItem ParentFolder { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<StorageItem> StorageItems { get; set; }
    }
}
