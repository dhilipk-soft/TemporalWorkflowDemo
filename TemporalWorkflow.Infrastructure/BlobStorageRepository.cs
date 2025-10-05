using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemporalWorkflow.Application.Interfaces;

namespace TemporalWorkflow.Infrastructure
{
    public class BlobStorageRepository : IBlobStorageRepository
    {
        private readonly IBlobContainerProvider blobContainerProvider;

        BlobStorageRepository(IBlobContainerProvider blobContainerProvider)
        {
            blobContainerProvider = blobContainerProvider;
        }

        public async Task<Stream> DownloadBlobAsync(string containerName, string blobName)
        {
            var container = blobContainerProvider.GetContainerClient(containerName);
            var blob = await container.GetBlobClient(blobName).DownloadAsync();

            return blob.Value.Content;
        }

        public async Task<List<string>> ListBlobsAsync(string containerName)
        {
            var container = blobContainerProvider.GetContainerClient(containerName);
            var blobs = new List<string>();
            await foreach( var b in container.GetBlobsAsync())
            {
                blobs.Add(b.Name);
            }
            return blobs;
        }

        public Task UploadBlobAsync(string containerName, string blobName, Stream stream)
        {
            var container = blobContainerProvider.GetContainerClient(containerName);

            container.GetBlobClient(blobName).UploadAsync(stream, overwrite:true);
            return Task.CompletedTask;
        }
    }
}
