using System;
using System.Threading.Tasks;
using Temporalio.Activities;
using TemporalWorkflowDemo.Models;

namespace TemporalWorkflowDemo
{
    public class OrderActivities
    {
        public OrderActivities()
        {
        }

        [Activity]
        public Task<OrderStatusResponse> ReserveItemAsync(Guid itemId, int quantity)
        {
            Console.WriteLine($"Reserving {quantity} of item {itemId}");
            return Task.FromResult(new OrderStatusResponse { WorkflowId = Guid.NewGuid(), Status = OrderStatus.Started });
        }

        [Activity]
        public Task<OrderStatusResponse> PackOrderAsync(Guid workflowId)
        {
            Console.WriteLine($"Packing order {workflowId}");
            return Task.FromResult(new OrderStatusResponse { WorkflowId = workflowId, Status = OrderStatus.Packed });
        }

        [Activity]
        public Task<OrderStatusResponse> DispatchOrderAsync(Guid workflowId)
        {
            Console.WriteLine($"Dispatching order {workflowId}");
            return Task.FromResult(new OrderStatusResponse { WorkflowId = workflowId, Status = OrderStatus.Dispatched });
        }

        [Activity]
        public Task<OrderStatusResponse> DeliverOrderAsync(Guid workflowId)
        {
            Console.WriteLine($"Delivering order {workflowId}");
            return Task.FromResult(new OrderStatusResponse { WorkflowId = workflowId, Status = OrderStatus.Reached });
        }

        [Activity]
        public Task<OrderStatusResponse> CompleteOrderAsync(Guid workflowId)
        {
            Console.WriteLine($"Completing order {workflowId}");
            return Task.FromResult(new OrderStatusResponse { WorkflowId = workflowId, Status = OrderStatus.Completed });
        }
    }
}