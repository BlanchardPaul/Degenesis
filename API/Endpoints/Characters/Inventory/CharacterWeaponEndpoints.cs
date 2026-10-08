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
            var result = await service.GetByCharacterIdAsync(characterId);
            return result.IsError ? Results.BadRequest(result) : Results.Ok(result);
        });
        group.MapPost("/", async (CharacterWeaponCreateDto characterWeaponCreate, ICharacterWeaponService service) =>
        {
            var result = await service.CreateAsync(characterWeaponCreate);
            return result.IsError ? Results.BadRequest(result) : Results.Created();
        });
        group.MapPut("/", async (CharacterWeaponDto characterWeaponDto, ICharacterWeaponService service) =>
        {
            var result = await service.UpdateAsync(characterWeaponDto);
            return result.IsError ? Results.BadRequest(result) : Results.Ok();
        });
        group.MapDelete("/{id:guid}", async (Guid id, ICharacterWeaponService service) =>
        {
            var result = await service.DeleteAsync(id);
            return result.IsError ? Results.BadRequest(result) : Results.NoContent();
        });
    }
}