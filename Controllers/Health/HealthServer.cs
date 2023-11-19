using Microsoft.AspNetCore.Mvc;

namespace skyhub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {

        [HttpGet]
        public IActionResult Ping()
        {
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error during ping: {ex.Message}");
            }
        }
    }
}
