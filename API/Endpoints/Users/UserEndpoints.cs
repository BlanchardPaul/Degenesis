using Business.Users;
using Degenesis.Shared.DTOs.Users;

namespace API.Endpoints.Users;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/users").WithTags("Users");

        group.MapPost("/register", async (UserCreateDto userCreateDto, IUserService userService) =>
        {
            var success = await userService.RegisterAsync(userCreateDto);
            return success ? Results.Ok("User registered successfully") : Results.BadRequest("Registration failed");
        });

        group.MapPost("/login", async (UserLoginDto userLoginDto, IUserService userService) =>
        {
            var token = await userService.LoginAsync(userLoginDto);
            return token is not null ? Results.Ok(token) : Results.Unauthorized();
        });
    }
}