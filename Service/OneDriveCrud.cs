using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Graph.Models.Security;
using OneDriveAmazoneS3.DTOs;
using OneDriveAmazoneS3.Service.Interface;

namespace OneDriveAmazoneS3.Service
{
    public class OneDriveCrud : IOneDriveCrud
    {
        private readonly IGraphClientFactory _graphClientFactory;
        public OneDriveCrud(IGraphClientFactory graphClientFactory)
        {
            _graphClientFactory = graphClientFactory;
        }
        public async Task<DownloardFileDto?> GetFileByNameAsync(string fileName) 
        {
            try
            {
                var graphClient = _graphClientFactory.CreateClient();

                // Target sender account
                string userEmail = "XXXXXXXXX.gmail.com";

                // 1. Get the user's OneDrive ID
                var drive = await graphClient
                    .Users[userEmail]
                    .Drive
                    .GetAsync();

                if (string.IsNullOrEmpty(drive?.Id))
                {
                    throw new FileNotFoundException("OneDrive not found.");
                }

                // 2. Download file directly using its path
                var stream = await graphClient
                    .Drives[drive.Id]
                    .Root
                    .ItemWithPath($"Documents/{fileName}")
                    .Content
                    .GetAsync();

                // Check if the stream is null
                if (stream == null)
                {
                    throw new FileNotFoundException($"File '{fileName}' not found.");
                }

                //return the file stream and metadata
                return new DownloardFileDto
                {
                    Stream = stream,
                    FileName = fileName,
                    ContentType = "application/octet-stream"
                };
            }
            catch (Microsoft.Graph.Models.ODataErrors.ODataError ex)
            {
                throw new FileNotFoundException( $"Unable to retrieve file '{fileName}' from OneDrive.", ex);
            }    
        }
        public async Task<DriveItem?> UploadFileAsync(SendFileOneDriveDto file)
        {
            try
            {
                var graphClient = _graphClientFactory.CreateClient();

                // Target sender account
                string userEmail = "XXXXXXXXX.gmail.com";

                //  Get the user's OneDrive ID
                var drive = await graphClient
                    .Users[userEmail]
                    .Drive
                    .GetAsync();

                if (string.IsNullOrEmpty(drive?.Id))
                {
                    throw new FileNotFoundException("OneDrive not found.");
                }

                // 1. Open uploaded file
                await using var stream = file.File.OpenReadStream();

                // 2. Upload file to Documents folder
                var driveItem = await graphClient
                    .Drives[drive.Id]
                    .Root
                    .ItemWithPath($"Documents/{file.File.FileName}")
                    .Content
                    .PutAsync(stream);

                return driveItem;
            }
            catch (Exception ex)
            {
                throw new Exception("Error uploading file to OneDrive.", ex);
            }
        }
    }
}
