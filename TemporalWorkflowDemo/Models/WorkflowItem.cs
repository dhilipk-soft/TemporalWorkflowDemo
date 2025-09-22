public class WorkflowItem
{
    public string id { get; set; } = Guid.NewGuid().ToString();  // required by Cosmos
    public string Name { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
