using FileStorage.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace FileStorage.Domain.DataTransferedObjects.StorageItemModels
{
    public class FolderCreateRequestDto
    {   
        [Required]
        [MaxLength(300)]
        public string DisplayName { get; set; }

        [Required]
        public bool IsFolder { get; set; } = true;

        public bool? IsRootFolder { get; set; }
        
        [Required]
        public User User { get; set; }

        public StorageItem ParentFolder { get; set; }
    }
}
