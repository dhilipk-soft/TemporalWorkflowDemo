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

    public async Task<WorkflowItem> AddItemAsync(WorkflowItem item)
    {
        if (string.IsNullOrEmpty(item.id))
            item.id = Guid.NewGuid().ToString();

        item.CreatedAt = DateTime.UtcNow;

        var response = await _container.CreateItemAsync(item, new PartitionKey(item.id));
        return response.Resource;
    }

    public async Task<WorkflowItem?> GetItemAsync(string id)
    {
        try
        {
            var response = await _container.ReadItemAsync<WorkflowItem>(
                id,
                new PartitionKey(id) // use value, not path
            );
            return response.Resource;
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
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
