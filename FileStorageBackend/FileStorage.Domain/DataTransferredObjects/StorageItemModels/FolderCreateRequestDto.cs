using FileStorage.Data.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace FileStorage.Domain.DataTransferredObjects.StorageItemModels
{
    public class FolderCreateRequestDto
    {   
        [Required]
        [MaxLength(300)]
        public string DisplayName { get; set; }

        [Required]
        public bool IsFolder { get; set; } = true;

        [Required]
        public bool IsPrimaryFolder { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        [Required]
        public User User { get; set; }

        public StorageItem ParentFolder { get; set; }
    }
}
