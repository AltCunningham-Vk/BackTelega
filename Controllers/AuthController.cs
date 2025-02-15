using BackTelega.Models;
using BackTelega.Services;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace BackTelega.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] BackTelega.Models.RegisterRequest request)
        {
            bool result = await _userService.RegisterUserAsync(request.Username, request.Email, request.Password);
            if (!result) return BadRequest("User already exists");
            return Ok("User registered successfully");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] BackTelega.Models.LoginRequest request)
        {
            string? token = await _userService.AuthenticateUserAsync(request.Username, request.Password);
            if (token == null) return Unauthorized("Invalid username or password");

            return Ok(new { token });
        }
    }
}
