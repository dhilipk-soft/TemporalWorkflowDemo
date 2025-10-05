﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemporalWorkflow.Application.Services.Azure
{
    public class AzureStorageSettings
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string ContainerName { get; set; } = string.Empty;
    }
}
