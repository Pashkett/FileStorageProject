using FileStorage.Data.Models;
using FileStorage.Domain.DataTransferredObjects.UserModels;
using System;

namespace FileStorage.Domain.DataTransferredObjects.StorageItemModels
{
    public class FileItemDto
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
        public string Extension { get; set; }
        public long Size { get; set; }
        public bool IsFolder { get; set; }
        public bool IsRecycled { get; set; }
        public bool IsPublic { get; set; }
        public UserDto User { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedOn { get; set; }

    }
}
