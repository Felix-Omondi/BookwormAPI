using System;
using Microsoft.EntityFrameworkCore;

namespace BookwormAPI.Models;

[Index(nameof(Token), IsUnique = true)]
[Index(nameof(UserId), IsUnique = true)]
[Index(nameof(ExpiresAt), IsUnique = true)]
public class RefreshToken
{
    public int Id { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsRevoked { get; set; }

    // Foreign key to User
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}
