using Microsoft.AspNetCore.Mvc;
using Temporalio.Client;
using TemporalWorkflowDemo.Models;

namespace TemporalWorkflowDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly TemporalClient _client;

        public OrderController(TemporalClient client)
        {
            _client = client;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderRequest orderRequest)
        {
            if (orderRequest.Quantity <= 0)
            {
                return BadRequest("Quantity must be greater than 0");
            }

            var workflowId = Guid.NewGuid().ToString();
            await _client.StartWorkflowAsync<OrderWorkflow>(
                wf => wf.RunAsync(orderRequest.ItemId, orderRequest.Quantity),
                new WorkflowOptions(id: workflowId, taskQueue: "order-task-queue")
            );

            return Ok(new { WorkflowId = "sdfalkj" });
        }

        //[HttpGet("{workflowId}/status")]
        //public async Task<IActionResult> GetOrderStatus(string workflowId)
        //{
        //    var handle = _client.GetWorkflowHandle<OrderWorkflow>(workflowId);
        //    var status = await handle.QueryAsync(wf => wf.GetStatusAsync());
        //    return Ok(status);
        //}
    }
}