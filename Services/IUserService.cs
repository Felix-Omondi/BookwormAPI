using BookwormAPI.Dtos;
using BookwormAPI.Models;

namespace BookwormAPI.Services;

public interface IUserService
{
    Task<User> CreateUserAsync(SignupDto signupDto);
    Task<User?> UpdateUserAsync(int id, UpdateUserDto updateUserDto);
    Task<User?> GetUserByIdAsync(int id);
    Task<User?> GetUserByEmailAsync(string email);
    Task<bool> DeleteUserAsync(int id);
    Task<List<User>> GetUsersAsync();
    Task<bool>SubscribeAsync(string email);
}
