using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Temporalio.Client;
using TemporalWorkflowDemo.Workers;

var builder = WebApplication.CreateBuilder(args);


///builder.WebHost.UseUrls("http://localhost:5005", "https://localhost:5006");
// Add controllers and Swagger
builder.Services.AddControllers();

// Register TemporalClient (synchronously)
builder.Services.AddSingleton(sp =>
    TemporalClient.ConnectAsync(new TemporalClientConnectOptions
    {
        TargetHost = "localhost:7233",
        Namespace = "default"
    }).GetAwaiter().GetResult()
);

// Add worker hosted service
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
