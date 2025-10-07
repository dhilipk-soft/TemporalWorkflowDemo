using Azure.Storage.Blobs;
using TemporalWorkflow.Application.Interfaces;
using TemporalWorkflow.Application.Services.Azure;

namespace TemporalWorkflow.Services.Services
{
    public class BlobContainerProvider: IBlobContainerProvider
    {
        private readonly AzureStorageSettings _settings;

        public BlobContainerProvider(AzureStorageSettings serviceClient)
        {
            _settings = serviceClient;
        }

        public BlobContainerClient GetContainerClient(string key)
        {
            return new BlobContainerClient(_settings.ConnectionString, key);
        }

    }
}
