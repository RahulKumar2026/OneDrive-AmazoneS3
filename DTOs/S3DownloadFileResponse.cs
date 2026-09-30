namespace OneDriveAmazoneS3.DTOs
{
    public class S3DownloadFileResponse
    {
        public Stream file { get; set; } = Stream.Null;
        public string key { get; set; } = string.Empty;
        public string contentType { get; set; } = string.Empty;
        public string isMessage { get; set; } = string.Empty;
    }
}
