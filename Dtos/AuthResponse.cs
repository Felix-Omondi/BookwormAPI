namespace BookwormAPI.Dtos;

public class AuthResponse
{
    public UserDto User { get; set; }
    public TokenDto Tokens { get; set; }
}
