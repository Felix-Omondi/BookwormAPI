using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BookwormAPI.Dtos;
using BookwormAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookwormAPI.Controllers
{
    [Authorize(Roles = "ADMIN")]
    [Route("api/v1/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user is null) return NotFound();
            return Ok(user);
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetUsersAsync();
            return Ok(users);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto updateUserDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _userService.GetUserByIdAsync(id);
            if (user is null) return NotFound();

            var updatedUser = await _userService.UpdateUserAsync(id, updateUserDto);
            return Ok(updatedUser);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var deleteResult = await _userService.DeleteUserAsync(id);
            if (!deleteResult) return NotFound();
            return NoContent();
        }

        [HttpPost("subscribe")]
        public async Task<IActionResult> Subscribe()
        {
            var sub = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            // var userId = Convert.ToInt32(sub);
            if(!int.TryParse(sub,out var userId)) return Unauthorized();

            var subscribeResult = await _userService.SubscribeAsync(userId);
            if (!subscribeResult) return NotFound();
            return Ok(new { message = "Subscribed to newsletter successfully." });
        }
    }
}
