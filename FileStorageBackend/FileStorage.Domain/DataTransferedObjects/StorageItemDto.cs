using System;

namespace FileStorage.Domain.DataTransferedObjects
{
    public class StorageItemDto
    {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public bool IsFolder { get; set; }
            public bool IsRootFolder { get; set; }
            public string RelativePath { get; set; }
            public string RootFolderName { get; set; }
            public virtual UserDto UserDto { get; set; }
    }
}
