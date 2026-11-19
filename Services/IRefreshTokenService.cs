using System;
using BookwormAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace BookwormAPI.Services;

public interface IRefreshTokenService
{
    Task<(string, DateTime)> GenerateRefreshToken(User user);
}
