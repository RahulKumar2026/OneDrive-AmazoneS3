using OneDriveAmazoneS3.DTOs;

namespace OneDriveAmazoneS3.Service.Interface
{
    public interface IAmazoneCrud
    {
        Task<S3DownloadFileResponse> getFileAsync(S3DownloadFileRequest request);
        Task<string> uploadFileAsync(UploadFileS3 file);
        Task<string> deleteFileAsync(string key);
    }
}
