using Business.Weapons;
using Degenesis.Shared.DTOs.Weapons;

namespace API.Endpoints.Weapons;

public static class WeaponEndpoints
{
    public static void MapWeaponEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/weapons").WithTags("Weapons");

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
            var weaponCreated = await service.CreateWeaponAsync(weapon);
            if (weaponCreated is null)
                return Results.BadRequest();
            return Results.Created($"/weapons/{weaponCreated.Id}", weaponCreated);
        });

        group.MapPut("/", async (IWeaponService service, WeaponDto weapon) =>
        {
            var updatedWeapon = await service.UpdateWeaponAsync(weapon);
            return updatedWeapon ? Results.NoContent() : Results.NotFound();
        });

        group.MapDelete("/{id}", async (IWeaponService service, Guid id) =>
        {
            await service.DeleteWeaponAsync(id);
            return Results.NoContent();
        });
    }
}