using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TemporalWorkflowDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeedController : ControllerBase
    {
        private readonly CosmosService _cosmos;

        public SeedController(CosmosService cosmos)
        {
            _cosmos = cosmos;
        }

        [HttpPost("demo")]
        public async Task<IActionResult> SeedDemoData()
        {
            await _cosmos.SeedDemoDataAsync();
            return Ok("Demo data inserted successfully.");
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddWorkflow([FromBody] WorkflowItem item)
        {
            var created = await _cosmos.AddItemAsync(item);
            return Ok(created);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWorkflow(string id)
        {
            var item = await _cosmos.GetItemAsync(id);
            if (item == null)
                return NotFound();
            return Ok(item);
        }


    }

}
