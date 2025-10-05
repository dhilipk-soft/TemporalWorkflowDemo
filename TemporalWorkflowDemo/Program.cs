using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Azure.Cosmos;
using Temporalio.Client;
using TemporalWorkflowDemo.Workers;
using TemporalWorkflow.Application.Services.Azure;

var builder = WebApplication.CreateBuilder(args);


builder.Services.Configure<AzureStorageSettings>(
    builder.Configuration.GetSection("AzureStorage"));


///builder.WebHost.UseUrls("http://localhost:5005", "https://localhost:5006");
// Add controllers and Swagger
builder.Services.AddControllers();

var cosmosConfig = builder.Configuration.GetSection("CosmosDb");

// Register CosmosClient
builder.Services.AddSingleton(sp =>
{
    return new CosmosClient(
        cosmosConfig["AccountEndpoint"],
        cosmosConfig["AccountKey"]);
});

builder.Services.AddSingleton<CosmosService>();

// Register TemporalClient (synchronously)
builder.Services.AddSingleton(sp =>
    TemporalClient.ConnectAsync(new TemporalClientConnectOptions
    {
        TargetHost = "localhost:7233",
        Namespace = "default"
    }).GetAwaiter().GetResult()
);

//// Add worker hosted service
builder.Services.AddHostedService<TemporalWorkerHostedService>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
