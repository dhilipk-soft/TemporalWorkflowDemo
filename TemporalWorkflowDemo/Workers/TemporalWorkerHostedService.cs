using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Temporalio.Client;
using Temporalio.Worker;
using TemporalWorkflowDemo;

namespace TemporalWorkflowDemo.Workers
{
    public class TemporalWorkerHostedService : IHostedService
    {
        private readonly TemporalClient _client;
        private readonly ILogger<TemporalWorkerHostedService> _logger;
        private TemporalWorker? _worker;

        public TemporalWorkerHostedService(
            TemporalClient client,
            ILogger<TemporalWorkerHostedService> logger)
        {
            _client = client;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                var workerOptions = new TemporalWorkerOptions("order-task-queue");
                workerOptions.AddWorkflow<OrderWorkflow>();
                workerOptions.AddAllActivities(new OrderActivities());

                _worker = new TemporalWorker(_client, workerOptions);

                // Run worker in background so host startup is not blocked
                _ = Task.Run(async () =>
                {
                    try
                    {
                        _logger.LogInformation("Temporal Worker starting...");
                        await _worker.ExecuteAsync(cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error in Temporal Worker execution");
                    }
                });

                _logger.LogInformation("Temporal Worker hosted service started successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error starting Temporal Worker hosted service");
            }

            return Task.CompletedTask;
        }


        public Task StopAsync(CancellationToken cancellationToken)
        {
            try
            {
                _worker?.Dispose();
                _logger.LogInformation("Temporal Worker stopped.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error stopping Temporal Worker");
            }

            return Task.CompletedTask;
        }

    }
}