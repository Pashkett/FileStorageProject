using System;
using System.Collections.Generic;

namespace FileStorage.Data.Models
{
    public class StorageItem
    {
        public Guid Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public string TrustedName { get; set; }
        public string Extension { get; set; }
        public string DisplayName { get; set; }
        public long Size { get; set; }
        public bool IsFolder { get; set; }
        public bool IsPrimaryFolder { get; set; }
        public bool IsRecycled { get; set; }
        public bool IsPublic { get; set; }
        public string RelativePath { get; set; }
        public virtual StorageItem ParentFolder { get; set; }
        public virtual Guid? ParentFolderId { get; set; }
        public virtual User User { get; set; }
        public virtual Guid UserId { get; set; }
        public virtual ICollection<StorageItem> StorageItems { get; set; }
    }
}
