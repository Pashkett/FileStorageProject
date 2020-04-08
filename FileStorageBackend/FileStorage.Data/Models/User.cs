using System;
using System.Collections.Generic;
using System.Text;

namespace FileStorage.Data.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual ICollection<StorageItem> StorageItems { get; set; }
    }
}
