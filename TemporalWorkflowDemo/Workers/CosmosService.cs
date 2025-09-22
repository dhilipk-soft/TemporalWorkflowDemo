using Microsoft.Azure.Cosmos;

public class CosmosService
{
    private readonly Container _container;

    public CosmosService(CosmosClient client, IConfiguration config)
    {
        var databaseName = config["CosmosDb:DatabaseName"];
        var containerName = config["CosmosDb:ContainerName"];

        // Ensure DB and container exist
        var dbResponse = client.CreateDatabaseIfNotExistsAsync(databaseName).GetAwaiter().GetResult();
        dbResponse.Database.CreateContainerIfNotExistsAsync(
            new ContainerProperties
            {
                Id = containerName,
                PartitionKeyPath = config["CosmosDb:PartitionKeyPath"] ?? "/id"
            }).GetAwaiter().GetResult();

        _container = client.GetContainer(databaseName, containerName);
    }

    public async Task AddItemAsync<T>(T item, string partitionKey)
    {
        await _container.CreateItemAsync(item, new PartitionKey(partitionKey));
    }

    public async Task<T> GetItemAsync<T>(string id, string partitionKey)
    {
        var response = await _container.ReadItemAsync<T>(id, new PartitionKey(partitionKey));
        return response.Resource;
    }

    public async Task SeedDemoDataAsync()
    {
        var items = new List<WorkflowItem>
    {
        new WorkflowItem { Name = "Order Processing", Status = "Pending" },
        new WorkflowItem { Name = "Payment Workflow", Status = "Completed" },
        new WorkflowItem { Name = "Notification Workflow", Status = "Running" }
    };

        foreach (var item in items)
        {
            await _container.CreateItemAsync(item, new PartitionKey(item.id));
        }
    }

}
