using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;

namespace TemporalWorkflow.Application.Interfaces
{
    public interface IBlobContainerProvider
    {
        BlobContainerClient GetContainerClient(string key);
    }
}
