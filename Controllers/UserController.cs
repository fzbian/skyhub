using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using skyhub.Models;
using skyhub.Services;

namespace skyhub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAdminService _adminService;

        public UserController(IUserService userServices, IAdminService adminServices)
        {
            _userService = userServices;
            _adminService = adminServices;
        }

        [HttpPost("UploadImage")]
        public async Task<IActionResult> UploadImage([FromQuery] LoginModel loginModel, [FromForm] IFormFile file)
        {
            try
            {
                if (loginModel.Email == null || loginModel.Password == null)
                {
                    return BadRequest();
                }

                var user = await _adminService.ValidateUserAsync(loginModel.Email, loginModel.Password);

                if (user == null)
                {
                    return Unauthorized();
                }

                var image = await _userService.CreateImage(user, file);

                return Ok(image);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error during login: {ex.Message}");
            }
        }

        [HttpGet("GetImage/{publicId}")]
        public async Task<IActionResult> GetImage(string publicId)
        {
            try
            {
                var image = await _userService.GetImageByPublicId(Guid.Parse(publicId));

                if (image == null)
                {
                    return NotFound();
                }

                return Ok(image);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error during login: {ex.Message}");
            }
        }

        [HttpDelete("DeleteImage")]
        public async Task<IActionResult> DeleteImage([FromForm] string email, [FromForm] string password, [FromForm] string publicId)
        {
            try
            {
                if (email == null || password == null)
                {
                    return BadRequest();
                }

                var user = await _adminService.ValidateUserAsync(email, password);

                if (user == null)
                {
                    return Unauthorized();
                }

                await _userService.DeleteImageAsync(Guid.Parse(publicId));

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error during login: {ex.Message}");
            }
        }

        [HttpGet("GetUserImages")]
        public async Task<IActionResult> GetUserImages([FromQuery] LoginModel loginModel)
        {
            try
            {
                if (loginModel.Email == null || loginModel.Password == null)
                {
                    return BadRequest();
                }

                var user = await _adminService.ValidateUserAsync(loginModel.Email, loginModel.Password);

                if (user == null)
                {
                    return Unauthorized();
                }

                var images = await _userService.GetUserImages(loginModel.Email);

                return Ok(images);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error during login: {ex.Message}");
            }
        }
    }
}
