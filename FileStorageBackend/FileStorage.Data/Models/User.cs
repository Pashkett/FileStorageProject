using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace FileStorage.Data.Models
{
    public class User : IdentityUser<Guid>
    {
        public DateTime CreatedOn { get; set; }
        public virtual ICollection<StorageItem> StorageItems { get; set; }
    }
}
