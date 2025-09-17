using AutoMapper;
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
    Task<bool> RegisterAsync(UserCreateDto userCreateDto);
    Task<string?> LoginAsync(UserLoginDto userLoginDto);
}

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    public UserService(UserManager<ApplicationUser> userManager, IMapper mapper, IConfiguration configuration)
    {
        _userManager = userManager;
        _mapper = mapper;
        _configuration = configuration;
    }

    public async Task<bool> RegisterAsync(UserCreateDto userCreateDto)
    {
        var user = _mapper.Map<ApplicationUser>(userCreateDto);
        var result = await _userManager.CreateAsync(user, userCreateDto.Password);
        return result.Succeeded;
    }

    public async Task<string?> LoginAsync(UserLoginDto userLoginDto)
    {
        var user = await _userManager.FindByNameAsync(userLoginDto.UserName);
        if (user is null || !await _userManager.CheckPasswordAsync(user, userLoginDto.Password)) 
            return null;

        var tokenHandler = new JwtSecurityTokenHandler();
        var keyConfig = _configuration["Jwt:Key"];
        if(keyConfig is null)
            return null;
        var key = Encoding.UTF8.GetBytes(keyConfig);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.Role, "User")
            }),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = tokenHandler.WriteToken(token);
        return jwtToken;
    }
}