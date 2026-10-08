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
            var result = await userService.RegisterAsync(userCreateDto);
            return result.IsError ? Results.BadRequest(result) : Results.Ok(result);
        });

        group.MapPost("/login", async (UserLoginDto userLoginDto, IUserService userService) =>
        {
            var result = await userService.LoginAsync(userLoginDto);
            return result.IsError ? Results.BadRequest(result) : Results.Ok(result);
        });
    }
}