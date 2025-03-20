using System.ComponentModel.DataAnnotations;

namespace Degenesis.Shared.DTOs.Users;
public class RegisterDto
{
    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    [RegularExpression(@"^(?=.*\d).+$", ErrorMessage = "Password must contain at least one digit.")]
    public string Password { get; set; } = string.Empty;

    [Compare("Password", ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; } = string.Empty;
}