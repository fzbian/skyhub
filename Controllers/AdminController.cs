using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using skyhub.Models;
using skyhub.Services;

namespace skyhub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminServices)
        {
            _adminService = adminServices;
        }

        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                List<User> users = await _adminService.GetAllAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error during getting users: {ex.Message}");
            }
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> Create(User user)
        {
            try
            {
                await _adminService.CreateAsync(user);
                return CreatedAtAction(nameof(GetByEmail), new { email = user.Email }, user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error during creating user: {ex.Message}");
            }
        }

        [HttpPut("UpdateUser/{email}")]
        public async Task<IActionResult> Update(string email, User user)
        {
            try
            {
                if (email != user.Email)
                {
                    return BadRequest();
                }
                await _adminService.UpdateAsync(email, user);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error during updating user: {ex.Message}");
            }
        }

        [HttpDelete("DeleteUser/{email}")]
        public async Task<IActionResult> Delete(string email)
        {
            try
            {
                User user = await _adminService.GetByEmailAsync(email);
                if (user == null)
                {
                    return NotFound();
                }
                await _adminService.DeleteAsync(email);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error during deleting user: {ex.Message}");
            }
        }

        [HttpGet("GetUserByEmail/{email}")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            try
            {
                User user = await _adminService.GetByEmailAsync(email);
                if (user == null)
                {
                    return NotFound();
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error during getting user: {ex.Message}");
            }
        }
    }
}
