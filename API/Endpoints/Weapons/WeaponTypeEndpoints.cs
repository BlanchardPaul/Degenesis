using Business.Weapons;
using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Weapons;

namespace API.Endpoints.Weapons;

public static class WeaponTypeEndpoints
{
    public static void MapWeaponTypeEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/weapon-types").WithTags("WeaponTypes").RequireAuthorization();

        group.MapGet("/{id}", async (IWeaponTypeService service, Guid id) =>
        {
            var result = await service.GetWeaponTypeByIdAsync(id);
            return result.IsError ? Results.BadRequest(result) : Results.Ok(result);
        });

        group.MapGet("/", async (IWeaponTypeService service) =>
        {
            var result = await service.GetAllWeaponTypesAsync();
            return result.IsError ? Results.BadRequest(result) : Results.Ok(result);
        });

        group.MapPost("/", async (IWeaponTypeService service, WeaponTypeCreateDto weaponType) =>
        {
            var result = await service.CreateWeaponTypeAsync(weaponType);
            return result.IsError ? Results.BadRequest(result) : Results.Created();
        });

        group.MapPut("/", async (IWeaponTypeService service, WeaponTypeDto weaponType) =>
        {
            var result = await service.UpdateWeaponTypeAsync(weaponType);
            return result.IsError ? Results.BadRequest(result) : Results.Ok();
        });

        group.MapDelete("/{id}", async (IWeaponTypeService service, Guid id) =>
        {
            var result = await service.DeleteWeaponTypeAsync(id);
            return result.IsError ? Results.BadRequest(result) : Results.NoContent();
        });
    }
}