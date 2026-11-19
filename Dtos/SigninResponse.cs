using System;

namespace BookwormAPI.Dtos;

public class SigninResponse
{
    public string Status { get; set; } = "success";
    public string Message { get; set; } = "Login successful.";
    public AuthResponse? Data { get; set; }
}
