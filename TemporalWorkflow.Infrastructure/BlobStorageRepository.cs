
using TemporalWorkflow.Application.Interfaces;
using TemporalWorkflow.Application.Services.Azure;

namespace TemporalWorkflow.Infrastructure
{
    public class BlobStorageRepository : IBlobStorageRepository
    {
        private readonly IBlobContainerProvider _blobContainerProvider;
        private readonly AzureStorageSettings _setting;

        public BlobStorageRepository(IBlobContainerProvider blobContainerProvider, AzureStorageSettings azureStorageSettings)
        {
            _blobContainerProvider = blobContainerProvider;
            _setting = azureStorageSettings;
        }

        public async Task<Stream> DownloadBlobAsync(string containerName, string blobName)
        {
            var container = _blobContainerProvider.GetContainerClient(_setting.Containers[containerName]);
            var blob = await container.GetBlobClient(blobName).DownloadAsync();

            return blob.Value.Content;
        }

        public async Task<List<string>> ListBlobsAsync(string containerName)
        {
            var container = _blobContainerProvider.GetContainerClient(_setting.Containers[containerName]);
            var blobs = new List<string>();
            await foreach( var b in container.GetBlobsAsync())
            {
                blobs.Add(b.Name);
            }
            return blobs;
        }

        public async Task UploadBlobAsync(string containerName, string blobName, Stream stream)
        {
            var container = _blobContainerProvider.GetContainerClient(_setting.Containers[containerName]);

            await container.GetBlobClient(blobName).UploadAsync(stream, overwrite:true); 
        }
    }
}
