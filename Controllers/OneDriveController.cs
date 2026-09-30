using Microsoft.AspNetCore.Mvc;
using OneDriveAmazoneS3.DTOs;
using OneDriveAmazoneS3.Service.Interface;

namespace OneDriveAmazoneS3.Controllers
{
    [ApiController]
    [Route("api/onedrive")]
    public class OneDriveController : ControllerBase
    {
        private readonly IOneDriveCrud _oneDriveCrud;
        public OneDriveController(IOneDriveCrud oneDriveCrud)
        {
            _oneDriveCrud = oneDriveCrud;
        }
        [HttpGet]
        [Route("file-download")]
        public async Task<IActionResult> GetFileByNameAsync(string filename) 
        {
            try
            {
                var file = await _oneDriveCrud.GetFileByNameAsync(filename);
                return File(file.Stream, file.ContentType, file.FileName);  
            }
            catch (Exception) 
            {
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }
        [HttpPost]
        [Route("file-upload")]
        public async Task<IActionResult> UploadFileAsync(SendFileOneDriveDto file)
        {
            try
            {
                await _oneDriveCrud.UploadFileAsync(file);
                return Ok("File uploaded successfully.");
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }
    }
}
