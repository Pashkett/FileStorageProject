using System;
using System.ComponentModel.DataAnnotations;

namespace FileStorage.Domain.DataTransferredObjects.UserModels
{
    public class UserForRegisterDto
    {
        
        [Required]
        [MaxLength(250)]
        public string UserName { get; set; }
        
        [Required]
        [StringLength(8, MinimumLength =4, ErrorMessage ="You must specify the password between 4 and 8 characters")]
        public string Password { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }
}
