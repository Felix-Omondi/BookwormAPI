namespace BookwormAPI.Dtos;

public class SignupDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    // public string Username { get; set; } // TODO: implement automatic username generation
    public string Email { get; set; }

    // [JsonConverter(typeof(JsonStringEnumConverter))]
    public string Role { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
}
