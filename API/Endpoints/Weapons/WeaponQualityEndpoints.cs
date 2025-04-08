using Business.Weapons;
using Degenesis.Shared.DTOs.Weapons;

namespace API.Endpoints.Weapons;

public static class WeaponQualityEndpoints
{
    public static void MapWeaponQualityEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/weapon-qualities").WithTags("WeaponQualities").RequireAuthorization();

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
            var created =  await service.CreateWeaponQualityAsync(weaponQuality);
            if (created is null)
                return Results.BadRequest();
            return Results.Created();
        });

        group.MapPut("/", async (IWeaponQualityService service, WeaponQualityDto weaponQuality) =>
        {
            var success = await service.UpdateWeaponQualityAsync(weaponQuality);
            return success ? Results.Ok() : Results.NotFound();
        });

        group.MapDelete("/{id}", async (IWeaponQualityService service, Guid id) =>
        {
            var success = await service.DeleteWeaponQualityAsync(id);
            return success ? Results.NoContent() : Results.NotFound();
        });
    }
}