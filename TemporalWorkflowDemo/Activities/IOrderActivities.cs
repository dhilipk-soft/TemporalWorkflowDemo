using System;
using System.Threading.Tasks;
using Temporalio.Activities;
using TemporalWorkflowDemo.Models;

public interface IOrderActivities
{
    [Activity]
    Task<OrderStatusResponse> ReserveItemAsync(Guid itemId, int quantity);

    [Activity]
    Task<OrderStatusResponse> PackOrderAsync(Guid workflowId);

    [Activity]
    Task<OrderStatusResponse> DispatchOrderAsync(Guid workflowId);

    [Activity]
    Task<OrderStatusResponse> DeliverOrderAsync(Guid workflowId);

    [Activity]
    Task<OrderStatusResponse> CompleteOrderAsync(Guid workflowId);
}
