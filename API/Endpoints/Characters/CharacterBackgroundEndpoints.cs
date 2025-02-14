using Business.Characters;
using Domain.Characters;

namespace API.Endpoints.Characters;

public static class CharacterBackgroundEndpoints
{
    public static void MapCharacterBackgroundEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/character-backgrounds").WithTags("Character Backgrounds");

        // GET /character-backgrounds/{characterId}/{backgroundId}
        group.MapGet("/{characterId:guid}/{backgroundId:guid}", async (Guid characterId, Guid backgroundId, ICharacterBackgroundService service) =>
        {
            var characterBackground = await service.GetCharacterBackgroundByIdAsync(characterId, backgroundId);
            return characterBackground is not null ? Results.Ok(characterBackground) : Results.NotFound();
        });

        // GET /character-backgrounds/character/{characterId}
        group.MapGet("/character/{characterId:guid}", async (Guid characterId, ICharacterBackgroundService service) =>
        {
            var characterBackgrounds = await service.GetCharacterBackgroundsByCharacterIdAsync(characterId);
            return Results.Ok(characterBackgrounds);
        });

        // GET /character-backgrounds/background/{backgroundId}
        group.MapGet("/background/{backgroundId:guid}", async (Guid backgroundId, ICharacterBackgroundService service) =>
        {
            var characterBackgrounds = await service.GetCharacterBackgroundsByBackgroundIdAsync(backgroundId);
            return Results.Ok(characterBackgrounds);
        });

        // POST /character-backgrounds
        group.MapPost("/", async (CharacterBackground characterBackground, ICharacterBackgroundService service) =>
        {
            var created = await service.CreateCharacterBackgroundAsync(characterBackground);
            return Results.Created($"/character-backgrounds/{created.CharacterId}/{created.BackgroundId}", created);
        });

        // PUT /character-backgrounds/{characterId}/{backgroundId}
        group.MapPut("/{characterId:guid}/{backgroundId:guid}", async (Guid characterId, Guid backgroundId, CharacterBackground characterBackground, ICharacterBackgroundService service) =>
        {
            var success = await service.UpdateCharacterBackgroundAsync(characterId, backgroundId, characterBackground);
            return success ? Results.NoContent() : Results.NotFound();
        });

        // DELETE /character-backgrounds/{characterId}/{backgroundId}
        group.MapDelete("/{characterId:guid}/{backgroundId:guid}", async (Guid characterId, Guid backgroundId, ICharacterBackgroundService service) =>
        {
            var success = await service.DeleteCharacterBackgroundAsync(characterId, backgroundId);
            return success ? Results.NoContent() : Results.NotFound();
        });
    }
}
