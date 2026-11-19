namespace BookwormAPI.Dtos;

public class UserDto
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    // public string Username { get; set; } // TODO: implement automatic username generation
    public string Email { get; set; }
    public string Role { get; set; }
    // public string Password { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
