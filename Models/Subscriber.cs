using Microsoft.EntityFrameworkCore;

namespace BookwormAPI.Models;

[Index(nameof(Email), IsUnique = true)]
public class Subscriber
{
    public int Id { get; set; }
    public string Email { get; set; }
    public bool IsPlatformUser { get; set; }
    public bool IsActive { get; set; }
    public DateTime SubscribedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UnsubscribedAt { get; set; }

    // Optional link to User if they have an account
    public int? UserId { get; set; }
    public User? User { get; set; }
}
