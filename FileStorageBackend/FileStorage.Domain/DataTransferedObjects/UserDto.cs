using System;
using System.Collections.Generic;

namespace FileStorage.Domain.DataTransferedObjects
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public virtual ICollection<StorageItemDto> StorageItems { get; set; }
    }
}
