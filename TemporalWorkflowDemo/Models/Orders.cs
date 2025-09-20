using Temporalio.Api.Enums.V1;

namespace TemporalWorkflowDemo.Models
{
    public class Orders
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string CustomerName { get; set; } = string.Empty;
        public OrderStatus Status { get; set; } = OrderStatus.Placed;

        public List<Item> Items { get; set; } = new();
    }
}
