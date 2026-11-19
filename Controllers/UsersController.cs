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

        // [HttpPost]
        // public async Task<IActionResult> AddUser(SignupDto signupDto)
        // {
        //     if (!ModelState.IsValid) return BadRequest(ModelState);

        //     var createdUser = await _userService.CreateUserAsync(signupDto);
        //     return CreatedAtAction(nameof(GetUser), new { id = createdUser.Id }, createdUser);
        // }

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
            // var user = await _userService.GetUserByIdAsync(id);
            // if (user is null) return NotFound();
            var deleteResult = await _userService.DeleteUserAsync(id);
            if (!deleteResult) return NotFound();
            return NoContent();
        }
    }
}
