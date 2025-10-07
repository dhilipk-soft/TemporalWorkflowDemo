using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemporalWorkflow.Application.Services.Azure
{
    public class AzureStorageSettings
    {
        public required string ConnectionString { get; set; }
        public required Dictionary<string, string> Containers { get; set; }
    }
}
