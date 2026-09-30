using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;
using OneDriveAmazoneS3.DTOs;
using OneDriveAmazoneS3.Service.Interface;
using OneDriveAmazoneS3.Settings;
using System.Net.Mime;
using System.Runtime;

namespace OneDriveAmazoneS3.Service
{
    public class AmazoneCrud : IAmazoneCrud
    {
        private readonly IAmazonS3 _amazoneClient;
        private readonly ILogger<AmazoneCrud> _logger;
        private readonly AmazoneS3Settings _amazoneS3Settings;
        public AmazoneCrud(IAmazonS3 amazoneClient, ILogger<AmazoneCrud> logger, IOptions<AmazoneS3Settings> settings) 
        {
            _amazoneClient = amazoneClient;
            _logger = logger;
            _amazoneS3Settings = settings.Value;
        }
        public async Task<S3DownloadFileResponse> getFileAsync(S3DownloadFileRequest request) 
        {
            try
            {
                var result = await _amazoneClient.GetObjectAsync(_amazoneS3Settings.BucketName, request.key);

                if (result == null)
                {
                    _logger.LogError("File not found");

                    return new S3DownloadFileResponse
                    {
                        file = null,
                        key = string.Empty,
                        contentType = string.Empty,
                        isMessage = "File not found"
                    }; 
                }
                return new S3DownloadFileResponse()
                {
                    file = result.ResponseStream,
                    key = result.Key,
                    contentType = result.Headers.ContentType,
                    isMessage = "File found"
                };
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error occurred while fetching file");
                throw;
            }
        }
        public async Task<string> uploadFileAsync(UploadFileS3 file) 
        {
            try
            {
                string key = file.file.FileName;
                using var stream = file.file.OpenReadStream();
                var request = new PutObjectRequest 
                {
                    InputStream = stream,
                    ContentType = file.file.ContentType,
                    BucketName = _amazoneS3Settings.BucketName,
                    Key = key
                };
                var response = await _amazoneClient.PutObjectAsync(request);
                return key;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error occurred while uploading file");
                throw;
            }
        }
        public async Task<string> deleteFileAsync(string key)
        {
            try
            {
                var request = new DeleteObjectRequest
                {
                    BucketName = _amazoneS3Settings.BucketName,
                    Key = key
                };
                var response = await _amazoneClient.DeleteObjectAsync(request);
                return key;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting file");
                throw;
            }
        }
    }
}
