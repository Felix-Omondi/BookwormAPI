using BookwormAPI.Models;

namespace BookwormAPI.Services;

public interface IRefreshTokenService
{
    Task<(string, DateTime)> GenerateRefreshToken(User user);
}
