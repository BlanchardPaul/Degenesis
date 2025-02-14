using Business.Weapons;
using Domain.Weapons;

namespace API.Endpoints.Weapons;

public static class WeaponQualityEndpoints
{
    public static void MapWeaponQualityEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/weaponQualities").WithTags("WeaponQualities");

        group.MapGet("/{id}", async (IWeaponQualityService service, Guid id) =>
        {
            var weaponQuality = await service.GetWeaponQualityByIdAsync(id);
            return weaponQuality is not null ? Results.Ok(weaponQuality) : Results.NotFound();
        });

        group.MapGet("/", async (IWeaponQualityService service) =>
        {
            var weaponQualities = await service.GetAllWeaponQualitiesAsync();
            return Results.Ok(weaponQualities);
        });

        group.MapPost("/", async (IWeaponQualityService service, WeaponQuality weaponQuality) =>
        {
            await service.CreateWeaponQualityAsync(weaponQuality);
            return Results.Created($"/weaponQualities/{weaponQuality.Id}", weaponQuality);
        });

        group.MapPut("/{id}", async (IWeaponQualityService service, Guid id, WeaponQuality weaponQuality) =>
        {
            await service.UpdateWeaponQualityAsync(id, weaponQuality);
            return Results.NoContent();
        });

        group.MapDelete("/{id}", async (IWeaponQualityService service, Guid id) =>
        {
            await service.DeleteWeaponQualityAsync(id);
            return Results.NoContent();
        });
    }
}