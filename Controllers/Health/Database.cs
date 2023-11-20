using Microsoft.AspNetCore.Mvc;
using skyhub.Services;

namespace skyhub.Controllers
{
    [Route("api/Health/[controller]")]
    [ApiController]
    public class DatabaseController : ControllerBase
    {
        private readonly MongoDBService _mongoDBService;

        public DatabaseController(MongoDBService mongoDBService)
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
                    return Ok();
                }
                else
                {
                    return StatusCode(500);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error during database ping: {ex.Message}");
            }
        }
    }
}
