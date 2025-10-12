using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace BlobProcessingFunctionApp
{
    public class CapitalizeBlobFunction
    {
        private readonly ILogger _logger;

        public CapitalizeBlobFunction(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<CapitalizeBlobFunction>();
        }

        [Function("CapitalizeBlobFunction")]
        public async Task Run(
            [BlobTrigger("Incoming/{name}", Connection = "AzureWebJobsStorage")] Stream incomingBlob,
            string name)
        {
            _logger.LogInformation($"Processing blob: {name}, Size: {incomingBlob.Length} bytes");

            // Read the incoming blob content
            using var reader = new StreamReader(incomingBlob);
            string content = await reader.ReadToEndAsync();

            // Convert to uppercase
            string capitalized = content.ToUpperInvariant();

            // Prepare destination container (processed)
            var connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
            var processedContainer = new BlobContainerClient(connectionString, "processed");
            await processedContainer.CreateIfNotExistsAsync();

            // Upload the modified file
            var blobClient = processedContainer.GetBlobClient(name);
            using var outputStream = new MemoryStream(Encoding.UTF8.GetBytes(capitalized));
            await blobClient.UploadAsync(outputStream, overwrite: true);

            _logger.LogInformation($"✅ Blob '{name}' processed and saved to 'processed' container.");
        }
    }
}
