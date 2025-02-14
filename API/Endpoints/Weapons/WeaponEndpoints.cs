using Business.Weapons;
using Domain.Weapons;

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

        group.MapPost("/", async (IWeaponService service, Weapon weapon) =>
        {
            await service.CreateWeaponAsync(weapon);
            return Results.Created($"/weapons/{weapon.Id}", weapon);
        });

        group.MapPut("/{id}", async (IWeaponService service, Guid id, Weapon weapon) =>
        {
            await service.UpdateWeaponAsync(id, weapon);
            return Results.NoContent();
        });

        group.MapDelete("/{id}", async (IWeaponService service, Guid id) =>
        {
            await service.DeleteWeaponAsync(id);
            return Results.NoContent();
        });
    }
}