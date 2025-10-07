using Autofac;
using TemporalWorkflow.Application.Interfaces;
using TemporalWorkflow.Application.Services.Azure;
using TemporalWorkflow.Infrastructure;
using TemporalWorkflow.Services.Services;
namespace TemporalWorkflow.API.Modules
{
    public class AzureStorageModule : Module
    {
        private readonly IConfiguration _configuration;

        public AzureStorageModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            // Initialize AzureStorageSettings with required properties
            var azureStorageSettings = new AzureStorageSettings
            {
                ConnectionString = _configuration.GetValue<string>("AzureStorage:ConnectionString"),
                Containers = _configuration.GetSection("AzureStorage:Containers").Get<Dictionary<string, string>>() ?? new Dictionary<string, string>()
            };

            builder.RegisterInstance(azureStorageSettings).SingleInstance();

            builder.RegisterType<BlobContainerProvider>().As<IBlobContainerProvider>().SingleInstance();
            builder.RegisterType<BlobStorageRepository>().As<IBlobStorageRepository>().InstancePerLifetimeScope();
            builder.RegisterType<AzureStorageService>().As<IAzureStorageService>().SingleInstance();
        }

    }
}
