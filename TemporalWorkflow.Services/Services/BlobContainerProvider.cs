using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemporalWorkflow.Application.Interfaces;

namespace TemporalWorkflow.Services.Services
{
    public class BlobContainerProvider: IBlobContainerProvider
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly IConfiguration _config;

        public BlobContainerProvider(BlobServiceClient serviceClient, IConfiguration config)
        {
            _blobServiceClient = serviceClient;
            _config = config;
        }

        public BlobContainerClient GetContainerClient(string key)
        {
            string container = _config[$"AzureStorage:Container:{key}"];

            if (string.IsNullOrEmpty(container))
            {
                throw new InvalidOperationException("Container mapping for '{key}' not found");
            }

            return _blobServiceClient.GetBlobContainerClient(container);
        }

    }
}
