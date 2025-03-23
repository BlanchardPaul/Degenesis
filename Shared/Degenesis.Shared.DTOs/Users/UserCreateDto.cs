using System.ComponentModel.DataAnnotations;

namespace Degenesis.Shared.DTOs.Users;
public class UserCreateDto
{
    [Required(ErrorMessage = "UserName is required.")]
    public string UserName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Must be an email")]
    public string Email { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    [RegularExpression(@"^(?=.*\d)(?=.*[A-Z])(?=.*[^a-zA-Z0-9]).{6,}$",
            ErrorMessage = "Password must be at least 6 characters, 1 digit, 1 uppercase et 1 non alphanumerical.")]
    public string Password { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; } = string.Empty;
}