using Business.Characters.Inventory;
using Degenesis.Shared.DTOs.Characters.CRUD.Inventory;

namespace API.Endpoints.Characters.Inventory;

public static class CharacterProtectionEndpoints
{
    public static void MapCharacterProtectionEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/character-protections").WithTags("CharacterProtections");

        group.MapGet("/character/{characterId:guid}", async (Guid characterId, ICharacterProtectionService service) =>
        {
            var result = await service.GetByCharacterIdAsync(characterId);
            return result.IsError ? Results.BadRequest(result) : Results.Ok(result);
        });
        group.MapPost("/", async (CharacterProtectionCreateDto characterProtection, ICharacterProtectionService service) =>
        {
            var result = await service.CreateAsync(characterProtection);
            return result.IsError ? Results.BadRequest(result) : Results.Created();
        });
        group.MapPut("/", async (CharacterProtectionDto characterProtection, ICharacterProtectionService service) =>
        {
            var result = await service.UpdateAsync(characterProtection);
            return result.IsError ? Results.BadRequest(result) : Results.Ok();
        });
        group.MapDelete("/{id:guid}", async (Guid id, ICharacterProtectionService service) =>
        {
            var result = await service.DeleteAsync(id);
            return result.IsError ? Results.BadRequest(result) : Results.NoContent();
        });
    }
}