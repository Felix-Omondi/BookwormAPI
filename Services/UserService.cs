using BookwormAPI.Data;
using BookwormAPI.Dtos;
using BookwormAPI.Models;
using CredGuard.Services;
using Microsoft.EntityFrameworkCore;

namespace BookwormAPI.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User> CreateUserAsync(SignupDto signupDto)
    {
        var hashedPassword = HashingService.HashPassword(signupDto.Password, 12);

        var newUser = new User
        {
            FirstName = signupDto.FirstName,
            LastName = signupDto.LastName,
            Email = signupDto.Email.Trim().ToLower(),
            Role = signupDto.Role,
            PasswordHash = hashedPassword,
        };

        newUser.Username = newUser.Email;

        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();
        return newUser;
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        var user = await _context.Users.Include(u => u.Books).FirstOrDefaultAsync(u => u.Id == id);
        if (user is null) return false;

        _context.Books.RemoveRange(user.Books);
        _context.Users.Remove(user);

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.Trim().ToLower());

        return user;
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        var user = await _context.Users.Include(u => u.Books).FirstOrDefaultAsync(u => u.Id == id);
        return user;
    }

    public async Task<List<User>> GetUsersAsync()
    {
        var users = await _context.Users.ToListAsync();
        return users;
    }

    public async Task<bool> SubscribeAsync(string email)
    {
        var user = await GetUserByEmailAsync(email);

        if (user is null) return false;

        if (!user.IsSubscribed)
        {
            user.IsSubscribed = true;
            await _context.SaveChangesAsync();
        }

        return true;
    }

    public async Task<User?> UpdateUserAsync(int id, UpdateUserDto updateUserDto)
    {
        var user = await _context.Users.FindAsync(id);

        if (user is not null)
        {
            if (!string.IsNullOrWhiteSpace(updateUserDto.FirstName))
            {
                user.FirstName = updateUserDto.FirstName;
            }

            if (!string.IsNullOrWhiteSpace(updateUserDto.LastName))
            {
                user.LastName = updateUserDto.LastName;
            }

            if (!string.IsNullOrWhiteSpace(updateUserDto.Email))
            {
                user.Email = updateUserDto.Email.Trim().ToLower();
            }

            if (!string.IsNullOrWhiteSpace(updateUserDto.Role))
            {
                user.Role = updateUserDto.Role;
            }

            if (!string.IsNullOrWhiteSpace(updateUserDto.Password))
            {
                var passwordHash = HashingService.HashPassword(updateUserDto.Password.Trim(), 12);
                user.PasswordHash = passwordHash;
            }

            await _context.SaveChangesAsync();
            return user;
        }

        return user;
    }
}
