using System.ComponentModel.DataAnnotations;

namespace BookwormAPI.Dtos;

public class SubscribeDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}
