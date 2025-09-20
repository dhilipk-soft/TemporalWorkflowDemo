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
        private TemporalWorker? _worker;

        public TemporalWorkerHostedService(TemporalClient client)
        {
            _client = client;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var workerOptions = new TemporalWorkerOptions("order-task-queue");

            // Register workflow
            workerOptions.AddWorkflow<OrderWorkflow>();

            // Register all activities
            workerOptions.AddAllActivities(new OrderActivities());

            _worker = new TemporalWorker(_client, workerOptions);

            await _worker.ExecuteAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _worker?.Dispose();
            return Task.CompletedTask;
        }
    }
}