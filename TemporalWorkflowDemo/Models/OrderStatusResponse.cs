namespace TemporalWorkflowDemo.Models
{
    public class OrderStatusResponse
    {
        public Guid WorkflowId { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Started;
    }
}
