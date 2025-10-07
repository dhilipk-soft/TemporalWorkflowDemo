
using Microsoft.Extensions.Logging;
using TemporalWorkflow.Application.Interfaces;

namespace TemporalWorkflow.Services.Services
{
    public class AzureStorageService : IAzureStorageService
    {
        private readonly ILogger<AzureStorageService> _logger;
        private readonly IBlobStorageRepository _blobStorageRepository;

        public AzureStorageService(IBlobStorageRepository blobStorageRepository, ILogger<AzureStorageService> logger )
        {
            _blobStorageRepository = blobStorageRepository;
            _logger = logger;
        }
        
        public async Task<Stream> DownloadBlobAsync(string containerName, string blobName)
        {
            _logger.LogInformation("Downloading blob {blobName} from container {container}", blobName, containerName);
            return await _blobStorageRepository.DownloadBlobAsync(containerName, blobName);
        }

        public async Task<List<string>> ListBlobsAsync(string container)
        {
            _logger.LogInformation("Listing blobs from container {container}", container);
            return await _blobStorageRepository.ListBlobsAsync(container);
        }

        public async Task UploadBlobAsync(string container, string blobName, Stream stream)
        {
            _logger.LogInformation("Uploading blob {blobName} to container {container}", blobName, container);
            await _blobStorageRepository.UploadBlobAsync(container, blobName, stream);
        }


    }
}
