using Business.Weapons;
using Domain.Weapons;

namespace API.Endpoints.Weapons;

public static class WeaponTypeEndpoints
{
    public static void MapWeaponTypeEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/weaponTypes").WithTags("WeaponTypes");

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

        group.MapPost("/", async (IWeaponTypeService service, WeaponType weaponType) =>
        {
            await service.CreateWeaponTypeAsync(weaponType);
            return Results.Created($"/weaponTypes/{weaponType.Id}", weaponType);
        });

        group.MapPut("/{id}", async (IWeaponTypeService service, Guid id, WeaponType weaponType) =>
        {
            await service.UpdateWeaponTypeAsync(id, weaponType);
            return Results.NoContent();
        });

        group.MapDelete("/{id}", async (IWeaponTypeService service, Guid id) =>
        {
            await service.DeleteWeaponTypeAsync(id);
            return Results.NoContent();
        });
    }
}