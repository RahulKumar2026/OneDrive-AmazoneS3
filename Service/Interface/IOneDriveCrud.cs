using Microsoft.Graph.Models;
using OneDriveAmazoneS3.DTOs;
namespace OneDriveAmazoneS3.Service.Interface
{
    public interface IOneDriveCrud
    {
        Task<DownloardFileDto?> GetFileByNameAsync(string fileName);
        Task<DriveItem?> UploadFileAsync(SendFileOneDriveDto file);
    }
}
