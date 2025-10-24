using Business.Characters;
using Degenesis.Shared.DTOs.Characters.CRUD;

namespace API.Endpoints.Characters;

public static class CharacterPotentialEndpoints
{
    public static void MapCharacterPotentialEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/character-potentials").WithTags("CharacterPotentials");

        group.MapPost("/", async (CharacterGuidValueEditDto potentialToCreate, ICharacterPotentialService service) =>
        {
            var success = await service.CreateCharacterPotentialAsync(potentialToCreate);
            return success ? Results.Ok() : Results.BadRequest();
        });

        group.MapPut("/", async (CharacterPotentialDto characterPotential, ICharacterPotentialService service) =>
        {
            var success = await service.UpdateCharacterPotentialAsync(characterPotential);
            return success ? Results.Ok() : Results.BadRequest();
        });

        group.MapDelete("/{characterId:guid}/{characterPotentialId:guid}", async (Guid characterId, Guid characterPotentialId, ICharacterPotentialService service) =>
        {
            var success = await service.DeleteCharacterPotentialAsync(characterId, characterPotentialId);
            return success ? Results.NoContent() : Results.NotFound();
        });
    }
}
