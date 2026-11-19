namespace BookwormAPI.Dtos;

public class UpdateUserDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    // public string? Username { get; set; }
    public string? Email { get; set; }

    // [JsonConverter(typeof(JsonStringEnumConverter))]
    public string Role { get; set; }
    public string? Password { get; set; }
}
