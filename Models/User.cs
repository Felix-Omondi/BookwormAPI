using Microsoft.EntityFrameworkCore;

namespace BookwormAPI.Models;

[Index(nameof(Email), IsUnique = true)]
// [Index(nameof(Username), IsUnique = true)]
[Index(nameof(Id), IsUnique = true)]
public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; } // TODO: implement automatic username generation
    public string Email { get; set; }
    public string Role { get; set; }
    public string PasswordHash { get; set; }
    // public bool IsSubscribed { get; set; }
    // public string? RefreshToken { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    // public DateTime? RefreshTokenExpiryTime { get; set; }
    public ICollection<Book> Books { get; set; } = new List<Book>();
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}
