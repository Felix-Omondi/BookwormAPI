namespace BookwormAPI.Dtos;

public class SignupResponse
{
    public string Status { get; set; } = "success";
    public string Message { get; set; } = "Account created successfully";
    public UserDto? User { get; set; }
}
