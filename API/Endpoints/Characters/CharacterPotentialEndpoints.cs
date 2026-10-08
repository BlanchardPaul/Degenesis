using Business.Characters;
using Degenesis.Shared.DTOs.Characters.CRUD;
using Domain.Characters;

namespace API.Endpoints.Characters;

public static class CharacterPotentialEndpoints
{
    public static void MapCharacterPotentialEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/character-potentials").WithTags("CharacterPotentials");

        group.MapPost("/", async (CharacterGuidValueEditDto potentialToCreate, ICharacterPotentialService service) =>
        {
            var result = await service.CreateCharacterPotentialAsync(potentialToCreate);
            return result.IsError ? Results.BadRequest(result) : Results.Created();
        });

        group.MapPut("/", async (CharacterPotentialDto characterPotential, ICharacterPotentialService service) =>
        {
            var result = await service.UpdateCharacterPotentialAsync(characterPotential);
            return result.IsError ? Results.BadRequest(result) : Results.Created();
        });

        group.MapDelete("/{characterId:guid}/{characterPotentialId:guid}", async (Guid characterId, Guid characterPotentialId, ICharacterPotentialService service) =>
        {
            var result = await service.DeleteCharacterPotentialAsync(characterId, characterPotentialId);
            return result.IsError ? Results.BadRequest(result) : Results.Created();
        });
    }
}
