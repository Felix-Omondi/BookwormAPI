using System;
using BookwormAPI.Data;
using BookwormAPI.Models;
using CredGuard.Services;
using Microsoft.AspNetCore.Identity;

namespace BookwormAPI.Services;

public class RefreshTokenService : IRefreshTokenService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public RefreshTokenService(AppDbContext context, IConfiguration configuration)
    {
        _configuration = configuration;
        _context = context;
    }

    public async Task<(string, DateTime)> GenerateRefreshToken(User user)
    {
        var tokenService = new TokenService(_configuration);
        var (refreshToken, refreshTokenExpiry) = tokenService.GenerateRefreshToken();

        var userRefreshToken = new RefreshToken
        {
            UserId = user.Id,
            Token = refreshToken,
            ExpiresAt = refreshTokenExpiry,
            IsRevoked = false
        };

        _context.RefreshTokens.Add(userRefreshToken);
        await _context.SaveChangesAsync();

        return (refreshToken, refreshTokenExpiry);
    }
}
