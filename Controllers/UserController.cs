using BackTelega.Models;
using BackTelega.Models.DTOs;
using BackTelega.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackTelega.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(new UserResponse
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                CreatedAt = user.CreatedAt
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            bool result = await _userService.DeleteUserAsync(id);
            if (!result) return NotFound();
            return Ok("User deleted successfully");
        }
    }
}
