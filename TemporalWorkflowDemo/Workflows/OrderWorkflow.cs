using System;
using System.Threading.Tasks;
using Temporalio.Workflows;
using TemporalWorkflowDemo.Models;

namespace TemporalWorkflowDemo
{
    [Workflow]
    public class OrderWorkflow
    {
        private OrderStatus _status = OrderStatus.Started;
        private readonly Guid _workflowId;

        public OrderWorkflow()
        {
            _workflowId = Guid.Parse(Workflow.Info.WorkflowId);
        }

        [WorkflowRun]
        public async Task<OrderStatusResponse> RunAsync(Guid itemId, int quantity)
        {
            _status = OrderStatus.Started;
            var reserve = await Workflow.ExecuteActivityAsync(
                (OrderActivities x) => x.ReserveItemAsync(itemId, quantity),
                new ActivityOptions { StartToCloseTimeout = TimeSpan.FromSeconds(10) });

            _status = OrderStatus.Reserved;
            var pack = await Workflow.ExecuteActivityAsync(
                (OrderActivities x) => x.PackOrderAsync(reserve.WorkflowId),
                new ActivityOptions { StartToCloseTimeout = TimeSpan.FromSeconds(10) });

            _status = OrderStatus.Packed;
            var dispatch = await Workflow.ExecuteActivityAsync(
                (OrderActivities x) => x.DispatchOrderAsync(pack.WorkflowId),
                new ActivityOptions { StartToCloseTimeout = TimeSpan.FromSeconds(10) });

            _status = OrderStatus.Dispatched;
            var deliver = await Workflow.ExecuteActivityAsync(
                (OrderActivities x) => x.DeliverOrderAsync(dispatch.WorkflowId),
                new ActivityOptions { StartToCloseTimeout = TimeSpan.FromSeconds(10) });

            _status = OrderStatus.Reached;
            var complete = await Workflow.ExecuteActivityAsync(
                (OrderActivities x) => x.CompleteOrderAsync(deliver.WorkflowId),
                new ActivityOptions { StartToCloseTimeout = TimeSpan.FromSeconds(10) });

            _status = OrderStatus.Completed;
            return complete;
        }

        //[WorkflowQuery]
        //public Task<OrderStatusResponse> GetStatusAsync()
        //{
        //    return Task.FromResult(new OrderStatusResponse
        //    {
        //        WorkflowId = _workflowId,
        //        Status = _status
        //    });
        //}
    }
}