using System.ComponentModel.DataAnnotations;

namespace OneDriveAmazoneS3.DTOs
{
    public class S3DownloadFileRequest
    {
        [Required]
        public string key { get; set; } = string.Empty;

    }
}
