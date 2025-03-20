using Business.Weapons;
using Degenesis.Shared.DTOs.Weapons;

namespace API.Endpoints.Weapons;

public static class WeaponQualityEndpoints
{
    public static void MapWeaponQualityEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/weapon-qualities").WithTags("WeaponQualities");

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

        group.MapPost("/", async (IWeaponQualityService service, WeaponQualityCreateDto weaponQuality) =>
        {
            var weaponQualityCreated =  await service.CreateWeaponQualityAsync(weaponQuality);
            if (weaponQualityCreated is null)
                return Results.BadRequest();
            return Results.Created($"/weapon-qualities/{weaponQualityCreated.Id}", weaponQualityCreated);
        });

        group.MapPut("/", async (IWeaponQualityService service, WeaponQualityDto weaponQuality) =>
        {
            var updatedWeaponQuality = await service.UpdateWeaponQualityAsync(weaponQuality);
            return updatedWeaponQuality ? Results.NoContent() : Results.NotFound();
        });

        group.MapDelete("/{id}", async (IWeaponQualityService service, Guid id) =>
        {
            await service.DeleteWeaponQualityAsync(id);
            return Results.NoContent();
        });
    }
}