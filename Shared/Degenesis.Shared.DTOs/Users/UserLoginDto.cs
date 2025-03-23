using System.ComponentModel.DataAnnotations;

namespace Degenesis.Shared.DTOs.Users;
public class UserLoginDto
{
    [Required(ErrorMessage = "UserName is required.")]
    public string UserName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}