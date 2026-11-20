namespace BookwormAPI.Dtos;

public class TokenDto
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public string TokenType { get; set; } = "Bearer";
    public DateTime AccessTokenExpirationTime  { get; set; }
}
