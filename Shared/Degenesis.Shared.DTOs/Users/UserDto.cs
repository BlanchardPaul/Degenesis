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

public class UserLoginDto
{
    [Required(ErrorMessage = "UserName is required.")]
    public string UserName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}

public class AccountDisplayDto
{
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

public class UpdateUserNameDto
{
    [Required(ErrorMessage = "UserName is required.")]
    public string CurrentUserName { get; set; } = string.Empty;

    [Required(ErrorMessage = "UserName is required.")]
    public string NewUserName { get; set; } = string.Empty;
}

public class UpdateEmailDto
{
    [Required(ErrorMessage = "UserName is required.")]
    public string UserName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Must be an email")]
    public string NewEmail { get; set; } = string.Empty;
}

public class UpdatePasswordDto
{
    [Required(ErrorMessage = "UserName is required.")]
    public string UserName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    public string CurrentPassword { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    [RegularExpression(@"^(?=.*\d)(?=.*[A-Z])(?=.*[^a-zA-Z0-9]).{6,}$",
            ErrorMessage = "Password must be at least 6 characters, 1 digit, 1 uppercase et 1 non alphanumerical.")]
    public string NewPassword { get; set; } = string.Empty;
}