using Degenesis.Shared.DTOs.Characters.CRUD.Inventory;

namespace API.Endpoints.Characters.Inventory;

public static class CharacterProtectionEndpoints
{
    public static void MapCharacterProtectionEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/character-protections").WithTags("CharacterProtections");

        group.MapGet("/character/{characterId:guid}", async (Guid characterId, ICharacterProtectionService service) =>
        {
            var protections = await service.GetByCharacterIdAsync(characterId);
            return protections.Any() ? Results.Ok(protections) : Results.NotFound();
        });
        group.MapPost("/", async (CharacterProtectionCreateDto characterProtection, ICharacterProtectionService service) =>
        {
            var created = await service.CreateAsync(characterProtection);
            return created is not null ? Results.Created() : Results.BadRequest();
        });
        group.MapPut("/", async (CharacterProtectionDto characterProtection, ICharacterProtectionService service) =>
        {
            var success = await service.UpdateAsync(characterProtection);
            return success ? Results.Ok() : Results.BadRequest();
        });
        group.MapDelete("/{id:guid}", async (Guid id, ICharacterProtectionService service) =>
        {
            var success = await service.DeleteAsync(id);
            return success ? Results.NoContent() : Results.NotFound();
        });
    }
}