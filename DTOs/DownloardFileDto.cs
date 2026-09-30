namespace OneDriveAmazoneS3.DTOs
{
    public class DownloardFileDto
    {
        public Stream Stream { get; set; }
        public string ContentType { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
    }
}
