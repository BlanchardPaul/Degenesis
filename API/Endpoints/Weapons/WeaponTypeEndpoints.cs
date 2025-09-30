using Business.Weapons;
using Degenesis.Shared.DTOs.Weapons;

namespace API.Endpoints.Weapons;

public static class WeaponTypeEndpoints
{
    public static void MapWeaponTypeEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/weapon-types").WithTags("WeaponTypes").RequireAuthorization();

        group.MapGet("/{id}", async (IWeaponTypeService service, Guid id) =>
        {
            var weaponType = await service.GetWeaponTypeByIdAsync(id);
            return weaponType is not null ? Results.Ok(weaponType) : Results.NotFound();
        });

        group.MapGet("/", async (IWeaponTypeService service) =>
        {
            var weaponTypes = await service.GetAllWeaponTypesAsync();
            return Results.Ok(weaponTypes);
        });

        group.MapPost("/", async (IWeaponTypeService service, WeaponTypeCreateDto weaponType) =>
        {
            var created = await service.CreateWeaponTypeAsync(weaponType);
            return created is not null ? Results.Created() : Results.BadRequest();
        });

        group.MapPut("/", async (IWeaponTypeService service, WeaponTypeDto weaponType) =>
        {
            var updatedWeaponType = await service.UpdateWeaponTypeAsync(weaponType);
            return updatedWeaponType ? Results.NoContent() : Results.NotFound();
        });

        group.MapDelete("/{id}", async (IWeaponTypeService service, Guid id) =>
        {
            var success = await service.DeleteWeaponTypeAsync(id);
            return success ? Results.NoContent() : Results.NotFound();
        });
    }
}