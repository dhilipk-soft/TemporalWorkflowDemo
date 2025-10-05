using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemporalWorkflow.Application.Interfaces
{
    public interface IBlobStorageRepository
    {
        Task UploadBlobAsync(string containerName, string blobName, Stream stream);
        Task<Stream> DownloadBlobAsync(string cotainerName, string blobName);
        Task<List<string>> ListBlobsAsync(string containerName);
    }
}
