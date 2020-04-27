using FileStorage.Data.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace FileStorage.Domain.DataTransferredObjects.StorageItemModels
{
    public class StorageItemDto
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(300)]
        public string DisplayName { get; set; }

        public bool IsFolder { get; set; }

        public bool IsRootFolder { get; set; }
        public StorageItemDto ParentFolder { get; set; }
    }
}
