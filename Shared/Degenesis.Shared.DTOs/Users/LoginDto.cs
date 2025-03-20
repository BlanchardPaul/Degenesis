using System.ComponentModel.DataAnnotations;

namespace Degenesis.Shared.DTOs.Users;
public class LoginDto
{
    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}