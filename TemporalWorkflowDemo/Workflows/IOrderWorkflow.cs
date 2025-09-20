using TemporalWorkflowDemo.Models;

namespace TemporalWorkflowDemo.Workflows
{
    public interface IOrderWorkflow
    {
        Task<OrderStatusResponse> GetStatusAsync();
    }
}
