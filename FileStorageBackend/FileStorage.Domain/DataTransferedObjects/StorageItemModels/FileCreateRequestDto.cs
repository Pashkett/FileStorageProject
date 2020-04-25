using Microsoft.AspNetCore.Http;
using FileStorage.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace FileStorage.Domain.DataTransferedObjects.StorageItemModels
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

        public bool? IsRootFolder { get; set; } = null;

        [Required]
        public User User { get; set; }
        
        [Required]
        public StorageItem ParentFolder { get; set; }
    }
}
