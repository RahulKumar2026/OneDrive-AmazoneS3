using Microsoft.AspNetCore.Mvc;
using OneDriveAmazoneS3.DTOs;
using OneDriveAmazoneS3.Service.Interface;

namespace OneDriveAmazoneS3.Controllers
{
    [ApiController]
    [Route("amazone")]
    public class AmazoneS3Controller : ControllerBase
    {
        private readonly IAmazoneCrud _amazoneCrud;
        public AmazoneS3Controller(IAmazoneCrud amazoneCrud) 
        {
            _amazoneCrud = amazoneCrud;
        }
       
        [HttpGet]
        [Route("download-file-s3")]
        public async Task<IActionResult> getFileAsync([FromQuery] S3DownloadFileRequest request)
        {
            try 
            {
                var result = await _amazoneCrud.getFileAsync(request);
                if(result == null || !result.isMessage.Equals("File found"))
                {
                    return NotFound("File not found");
                }
                return Ok(result);
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"Error occurred while fetching file Error: {ex.Message}");
            }
        }
        [HttpPost]
        [Route("upload-file-s3")]
        public async Task<IActionResult> uploadFileAsync([FromForm] UploadFileS3 file)
        {
            try
            {
                var result = await _amazoneCrud.uploadFileAsync(file);
                if (string.IsNullOrEmpty(result))
                {
                    return BadRequest("File upload failed");
                }
                return Ok(new { key = result, message = "File uploaded successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error occurred while uploading file Error: {ex.Message}");
            }
        }
        [HttpDelete]
        [Route("delete-file-s3")]
        public async Task<IActionResult> deleteFileAsync(string key) 
        {
            try 
            {
                var result = await _amazoneCrud.deleteFileAsync(key);
                if (string.IsNullOrEmpty(result))
                {
                    return NotFound("File not found or deletion failed");
                }
                return Ok(new { key = result, message = "File deleted successfully" });
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"Error occurred while deleting file Error: {ex.Message}");
            }
        }
    }
}
