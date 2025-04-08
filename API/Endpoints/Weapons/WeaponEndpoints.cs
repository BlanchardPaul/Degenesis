using Business.Weapons;
using Degenesis.Shared.DTOs.Weapons;

namespace API.Endpoints.Weapons;

public static class WeaponEndpoints
{
    public static void MapWeaponEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/weapons").WithTags("Weapons").RequireAuthorization();

        group.MapGet("/{id}", async (IWeaponService service, Guid id) =>
        {
            var weapon = await service.GetWeaponByIdAsync(id);
            return weapon is not null ? Results.Ok(weapon) : Results.NotFound();
        });

        group.MapGet("/", async (IWeaponService service) =>
        {
            var weapons = await service.GetAllWeaponsAsync();
            return Results.Ok(weapons);
        });

        group.MapPost("/", async (IWeaponService service, WeaponCreateDto weapon) =>
        {
            var created = await service.CreateWeaponAsync(weapon);
            if (created is null)
                return Results.BadRequest();
            return Results.Created();
        });

        group.MapPut("/", async (IWeaponService service, WeaponDto weapon) =>
        {
            var success = await service.UpdateWeaponAsync(weapon);
            return success ? Results.Ok() : Results.NotFound();
        });

        group.MapDelete("/{id}", async (IWeaponService service, Guid id) =>
        {
            var success = await service.DeleteWeaponAsync(id);
            return success ? Results.NoContent() : Results.NotFound();
        });
    }
}