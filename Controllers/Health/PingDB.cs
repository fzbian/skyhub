using Microsoft.AspNetCore.Mvc;
using skyhub.Services;

namespace skyhub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PingDBController : ControllerBase
    {
        private readonly MongoDBService _mongoDBService;

        public PingDBController(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        [HttpGet]
        public async Task<IActionResult> Ping()
        {
            try
            {
                bool isDatabaseConnected = await _mongoDBService.PingAsync();

                if (isDatabaseConnected)
                {
                    return Ok("Database is reachable");
                }
                else
                {
                    return StatusCode(500, "Database is not reachable");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error during database ping: {ex.Message}");
            }
        }
    }
}
