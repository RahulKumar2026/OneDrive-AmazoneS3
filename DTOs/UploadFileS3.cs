using System.ComponentModel.DataAnnotations;

namespace OneDriveAmazoneS3.DTOs
{
    public class UploadFileS3
    {
        [Required]
        public IFormFile file { get; set; } 
    }
}
