using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using skyhub.Models;
using skyhub.Services;
using Microsoft.AspNetCore.Authorization;

namespace skyhub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AuthController(IAdminService adminServices)
        {
            _adminService = adminServices;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            try
            {
                if (string.IsNullOrEmpty(loginModel.Email) || string.IsNullOrEmpty(loginModel.Password))
                {
                    return Unauthorized();
                }

                var user = await _adminService.ValidateUserAsync(loginModel.Email, loginModel.Password);

                if (user == null)
                {
                    return Unauthorized();
                }

                var token = GenerateJwtToken(user);

                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error during login: {ex.Message}");
            }
        }

        private string GenerateJwtToken(User user)
        {
            string? jwtAuthSecret = Environment.GetEnvironmentVariable("JWT_AUTH_SECRET");
            string? jwtAuthAudience = Environment.GetEnvironmentVariable("JWT_AUTH_AUDIENCE");
            string? jwtAuthIssuer = Environment.GetEnvironmentVariable("JWT_AUTH_ISSUER");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtAuthSecret ?? string.Empty));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtAuthIssuer,
                audience: jwtAuthAudience,
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public class LoginModel
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
