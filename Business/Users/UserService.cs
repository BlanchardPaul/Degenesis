using AutoMapper;
using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Users;
using Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Business.Users;
public interface IUserService
{
    Task<Result<object>> RegisterAsync(UserCreateDto userCreateDto);
    Task<Result<string>> LoginAsync(UserLoginDto userLoginDto);
    Task<Result<AccountDisplayDto>> GetAccountAsync(string userName);
    Task<Result<object>> UpdateUserNameAsync(UpdateUserNameDto dto);
    Task<Result<object>> UpdateEmailAsync(UpdateEmailDto dto);
    Task<Result<object>> UpdatePasswordAsync(UpdatePasswordDto dto);
}

public class UserService(UserManager<ApplicationUser> userManager, IMapper mapper, IConfiguration configuration) : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IMapper _mapper = mapper;
    private readonly IConfiguration _configuration = configuration;

    public async Task<Result<object>> RegisterAsync(UserCreateDto userCreateDto)
    {
        try
        {
            // Check if the user already exists
            var existingUser = await _userManager.FindByNameAsync(userCreateDto.UserName);
            if (existingUser != null)
                return new Result<object> { IsError = true, Error = "This name is already taken" };

            // Check if the email is already registered
            var existingEmailUser = await _userManager.FindByEmailAsync(userCreateDto.Email);
            if (existingEmailUser != null)
                return new Result<object> { IsError = true, Error = "This email address is already taken" };

            var user = _mapper.Map<ApplicationUser>(userCreateDto);
            var result = await _userManager.CreateAsync(user, userCreateDto.Password);
            return new Result<object> { Value = null };
        }
        catch(Exception)
        {
            return new Result<object> { IsError = true, Error = "An error occurred" };
        }

    }

    public async Task<Result<string>> LoginAsync(UserLoginDto userLoginDto)
    {
        try
        {
            var user = await _userManager.FindByNameAsync(userLoginDto.UserName);
            if (user == null)
                return new Result<string> { IsError = true, Error = "This user does not exist" };
            if (!await _userManager.CheckPasswordAsync(user, userLoginDto.Password))
                return new Result<string> { IsError = true, Error = "Invalid password" };

            var tokenHandler = new JwtSecurityTokenHandler();
            var keyConfig = _configuration["Jwt:Key"];
            if (keyConfig is null)
                return new Result<string> { IsError = true, Error = "JWT key is not configured" };
            var key = Encoding.UTF8.GetBytes(keyConfig);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                    new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.Role, "User")
                ]),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);
            return new Result<string> { Value = jwtToken };
        }
        catch(Exception)
        {
            return new Result<string> { IsError = true, Error = "An error occurred" };
        }
    }

    public async Task<Result<AccountDisplayDto>> GetAccountAsync(string userName)
    {
        try
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user is null)
                return new Result<AccountDisplayDto> { IsError = true, Error = "User not found" };

            return new Result<AccountDisplayDto>
            {
                Value = new AccountDisplayDto { UserName = user.UserName!, Email = user.Email! }
            };
        }
        catch (Exception)
        {
            return new Result<AccountDisplayDto> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> UpdateUserNameAsync(UpdateUserNameDto dto)
    {
        try
        {
            var user = await _userManager.FindByNameAsync(dto.CurrentUserName);
            if (user is null)
                return new Result<object> { IsError = true, Error = "User not found" };

            var existing = await _userManager.FindByNameAsync(dto.NewUserName);
            if (existing is not null)
                return new Result<object> { IsError = true, Error = "This name is already taken" };

            var result = await _userManager.SetUserNameAsync(user, dto.NewUserName);
            if (!result.Succeeded)
                return new Result<object> { IsError = true, Error = result.Errors.First().Description };

            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> UpdateEmailAsync(UpdateEmailDto dto)
    {
        try
        {
            var user = await _userManager.FindByNameAsync(dto.UserName);
            if (user is null)
                return new Result<object> { IsError = true, Error = "User not found" };

            var result = await _userManager.SetEmailAsync(user, dto.NewEmail);
            if (!result.Succeeded)
                return new Result<object> { IsError = true, Error = result.Errors.First().Description };

            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> UpdatePasswordAsync(UpdatePasswordDto dto)
    {
        try
        {
            var user = await _userManager.FindByNameAsync(dto.UserName);
            if (user is null)
                return new Result<object> { IsError = true, Error = "User not found" };

            // Verify the current password before allowing the change
            var passwordOk = await _userManager.CheckPasswordAsync(user, dto.CurrentPassword);
            if (!passwordOk)
                return new Result<object> { IsError = true, Error = "Current password is incorrect" };

            var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
            if (!result.Succeeded)
                return new Result<object> { IsError = true, Error = result.Errors.First().Description };

            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }
}