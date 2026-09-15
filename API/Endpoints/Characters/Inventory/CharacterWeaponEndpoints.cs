using Business.Characters.Inventory;
using Degenesis.Shared.DTOs.Characters.CRUD.Inventory;

namespace API.Endpoints.Characters.Inventory;

public static class CharacterWeaponEndpoints
{
    public static void MapCharacterWeaponEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/character-weapons").WithTags("CharacterWeapons");
        group.MapGet("/character/{characterId:guid}", async (Guid characterId, ICharacterWeaponService service) =>
        {
            var characterWeapons = await service.GetByCharacterIdAsync(characterId);
            return Results.Ok(characterWeapons);
        });
        group.MapPost("/", async (CharacterWeaponCreateDto characterWeaponCreate, ICharacterWeaponService service) =>
        {
            var createdCharacterWeapon = await service.CreateAsync(characterWeaponCreate);
            if (createdCharacterWeapon is null)
            {
                return Results.BadRequest("Failed to create CharacterWeapon.");
            }
            return Results.Created($"/character-weapons/{createdCharacterWeapon.Id}", createdCharacterWeapon);
        });
        group.MapPut("/", async (CharacterWeaponDto characterWeaponDto, ICharacterWeaponService service) =>
        {
            var success = await service.UpdateAsync(characterWeaponDto);
            if (!success)
            {
                return Results.NotFound();
            }
            return Results.Ok();
        });
        group.MapDelete("/{id:guid}", async (Guid id, ICharacterWeaponService service) =>
        {
            var success = await service.DeleteAsync(id);
            if (!success)
            {
                return Results.NotFound();
            }
            return Results.NoContent();
        });
    }
}