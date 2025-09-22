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

    }

}
