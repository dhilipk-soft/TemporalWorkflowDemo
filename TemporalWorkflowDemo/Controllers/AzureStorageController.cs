using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TemporalWorkflow.Application.Interfaces;

namespace TemporalWorkflow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AzureStorageController : ControllerBase
    {
        private readonly IAzureStorageService _azureStorageService;
        public AzureStorageController(IAzureStorageService azureStorageService)
        {
            _azureStorageService = azureStorageService;
        }

        [HttpGet("list/{container}")]
        public async Task<IActionResult> ListBlobs(string container)
        {
            var blobs = await _azureStorageService.ListBlobsAsync(container);
            return Ok(blobs);
        }

        [HttpGet("download/{container}/{blobName}")]
        public async Task<IActionResult> DownloadBlob(string container, string blobName)
        {
            var stream = await _azureStorageService.DownloadBlobAsync(container, blobName);
            if (stream == null)
            {
                return NotFound();
            }
            return File(stream, "application/octet-stream", blobName);
        }

        [HttpPost("upload/{container}/{blobName}")]
        public async Task<IActionResult> UploadBlob(string container, string blobName, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("File is empty");
            }
            using var stream = file.OpenReadStream();
            await _azureStorageService.UploadBlobAsync(container, blobName, stream);
            return Ok();
        }
    }
}
