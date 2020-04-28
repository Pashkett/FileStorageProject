using Microsoft.AspNetCore.Http;
using FileStorage.Data.Models;
using System.ComponentModel.DataAnnotations;
using System;

namespace FileStorage.Domain.DataTransferredObjects.StorageItemModels
{
    public class FileCreateRequestDto
    {
        [Required]
        [MaxLength(300)]
        public string TrustedName { get; set; }
        
        [Required]
        [MaxLength(300)]
        public string DisplayName { get; set; }

        public IFormFile File { get; set; }

        public bool IsFolder { get; set; } = false;

        public bool IsRootFolder { get; set; } = false;

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        [Required]
        public User User { get; set; }
        
        [Required]
        public StorageItem ParentFolder { get; set; }
    }
}
