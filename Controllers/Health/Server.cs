using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace skyhub.Controllers
{
    [Route("api/Health/[controller]")]
    [ApiController]
    public class ServerController : ControllerBase
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
