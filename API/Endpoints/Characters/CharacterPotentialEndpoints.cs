using Business.Characters;
using Domain.Characters;

namespace API.Endpoints.Characters;

public static class CharacterPotentialEndpoints
{
    public static void MapCharacterPotentialEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/character-potentials").WithTags("CharacterPotentials");

        // GET /character-potentials/{characterId}/{potentialId}
        group.MapGet("/{characterId:guid}/{potentialId:guid}", async (Guid characterId, Guid potentialId, ICharacterPotentialService service) =>
        {
            var characterPotential = await service.GetCharacterPotentialByIdAsync(characterId, potentialId);
            return characterPotential is not null ? Results.Ok(characterPotential) : Results.NotFound();
        });

        // GET /character-potentials/character/{characterId}
        group.MapGet("/character/{characterId:guid}", async (Guid characterId, ICharacterPotentialService service) =>
        {
            var characterPotentials = await service.GetCharacterPotentialsByCharacterIdAsync(characterId);
            return Results.Ok(characterPotentials);
        });

        // GET /character-potentials/potential/{potentialId}
        group.MapGet("/potential/{potentialId:guid}", async (Guid potentialId, ICharacterPotentialService service) =>
        {
            var characterPotentials = await service.GetCharacterPotentialsByPotentialIdAsync(potentialId);
            return Results.Ok(characterPotentials);
        });

        // POST /character-potentials
        group.MapPost("/", async (CharacterPotential characterPotential, ICharacterPotentialService service) =>
        {
            var created = await service.CreateCharacterPotentialAsync(characterPotential);
            return Results.Created($"/character-potentials/{created.CharacterId}/{created.PotentialId}", created);
        });

        // PUT /character-potentials/{characterId}/{potentialId}
        group.MapPut("/{characterId:guid}/{potentialId:guid}", async (Guid characterId, Guid potentialId, CharacterPotential characterPotential, ICharacterPotentialService service) =>
        {
            var success = await service.UpdateCharacterPotentialAsync(characterId, potentialId, characterPotential);
            return success ? Results.NoContent() : Results.NotFound();
        });

        // DELETE /character-potentials/{characterId}/{potentialId}
        group.MapDelete("/{characterId:guid}/{potentialId:guid}", async (Guid characterId, Guid potentialId, ICharacterPotentialService service) =>
        {
            var success = await service.DeleteCharacterPotentialAsync(characterId, potentialId);
            return success ? Results.NoContent() : Results.NotFound();
        });
    }
}
