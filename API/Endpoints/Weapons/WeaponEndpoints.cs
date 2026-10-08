using Business.Weapons;
using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Weapons;

namespace API.Endpoints.Weapons;

public static class WeaponEndpoints
{
    public static void MapWeaponEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/weapons").WithTags("Weapons").RequireAuthorization();

        group.MapGet("/{id}", async (IWeaponService service, Guid id) =>
        {
            var result = await service.GetWeaponByIdAsync(id);
            return result.IsError ? Results.BadRequest(result) : Results.Ok(result);
        });

        group.MapGet("/", async (IWeaponService service) =>
        {
            var result = await service.GetAllWeaponsAsync();
            return result.IsError ? Results.BadRequest(result) : Results.Ok(result);
        });

        group.MapPost("/", async (IWeaponService service, WeaponCreateDto weapon) =>
        {
            var result = await service.CreateWeaponAsync(weapon);
            return result.IsError ? Results.BadRequest(result) : Results.Created();
        });

        group.MapPut("/", async (IWeaponService service, WeaponDto weapon) =>
        {
            var result = await service.UpdateWeaponAsync(weapon);
            return result.IsError ? Results.BadRequest(result) : Results.Ok();
        });

        group.MapDelete("/{id}", async (IWeaponService service, Guid id) =>
        {
            var result = await service.DeleteWeaponAsync(id);
            return result.IsError ? Results.BadRequest(result) : Results.NotFound();
        });
    }
}