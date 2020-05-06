using System;

namespace FileStorage.Domain.DataTransferredObjects.StorageItemModels
{
    public class FileItemDto
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
        public string Extension { get; set; }
        public bool IsFolder { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedOn { get; set; }

    }
}
