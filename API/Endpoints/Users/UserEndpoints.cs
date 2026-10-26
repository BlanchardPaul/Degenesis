using Business.Users;
using Degenesis.Shared.DTOs.Users;

namespace API.Endpoints.Users;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/users").WithTags("Users");

        group.MapGet("/{userName}",
            async (string userName, IUserService service) =>
            {
                var result = await service.GetAccountAsync(userName);
                return result.IsError ? Results.BadRequest(result) : Results.Ok(result);
            });

        group.MapPut("/username",
            async (UpdateUserNameDto dto, IUserService service) =>
            {
                var result = await service.UpdateUserNameAsync(dto);
                return result.IsError ? Results.BadRequest(result) : Results.Ok();
            });

        group.MapPut("/email",
            async (UpdateEmailDto dto, IUserService service) =>
            {
                var result = await service.UpdateEmailAsync(dto);
                return result.IsError ? Results.BadRequest(result) : Results.Ok();
            });

        group.MapPut("/password",
            async (UpdatePasswordDto dto, IUserService service) =>
            {
                var result = await service.UpdatePasswordAsync(dto);
                return result.IsError ? Results.BadRequest(result) : Results.Ok();
            });

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