using Business.Characters;
using Domain.Characters;

namespace API.Endpoints.Characters;

public static class CharacterAttributeEndpoints
{
    public static void MapCharacterAttributeEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/character-attributes").WithTags("Character Attributes");

        // GET /character-attributes/{characterId}/{attributeId}
        group.MapGet("/{characterId:guid}/{attributeId:guid}", async (Guid characterId, Guid attributeId, ICharacterAttributeService service) =>
        {
            var characterAttribute = await service.GetCharacterAttributeByIdAsync(characterId, attributeId);
            return characterAttribute is not null ? Results.Ok(characterAttribute) : Results.NotFound();
        });

        // GET /character-attributes/character/{characterId}
        group.MapGet("/character/{characterId:guid}", async (Guid characterId, ICharacterAttributeService service) =>
        {
            var characterAttributes = await service.GetCharacterAttributesByCharacterIdAsync(characterId);
            return Results.Ok(characterAttributes);
        });

        // GET /character-attributes/attribute/{attributeId}
        group.MapGet("/attribute/{attributeId:guid}", async (Guid attributeId, ICharacterAttributeService service) =>
        {
            var characterAttributes = await service.GetCharacterAttributesByAttributeIdAsync(attributeId);
            return Results.Ok(characterAttributes);
        });

        // POST /character-attributes
        group.MapPost("/", async (CharacterAttribute characterAttribute, ICharacterAttributeService service) =>
        {
            var created = await service.CreateCharacterAttributeAsync(characterAttribute);
            return Results.Created($"/character-attributes/{created.CharacterId}/{created.AttributeId}", created);
        });

        // PUT /character-attributes/{characterId}/{attributeId}
        group.MapPut("/{characterId:guid}/{attributeId:guid}", async (Guid characterId, Guid attributeId, CharacterAttribute characterAttribute, ICharacterAttributeService service) =>
        {
            var success = await service.UpdateCharacterAttributeAsync(characterId, attributeId, characterAttribute);
            return success ? Results.NoContent() : Results.NotFound();
        });

        // DELETE /character-attributes/{characterId}/{attributeId}
        group.MapDelete("/{characterId:guid}/{attributeId:guid}", async (Guid characterId, Guid attributeId, ICharacterAttributeService service) =>
        {
            var success = await service.DeleteCharacterAttributeAsync(characterId, attributeId);
            return success ? Results.NoContent() : Results.NotFound();
        });
    }
}
