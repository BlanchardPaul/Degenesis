using Business.Characters;
using Degenesis.Shared.DTOs.Characters;

namespace API.Endpoints.Characters;

public static class CharacterPotentialEndpoints
{
    public static void MapCharacterPotentialEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/character-potentials").WithTags("CharacterPotentials");

        group.MapPut("/", async (CharacterPotentialDto characterPotential, ICharacterPotentialService service) =>
        {
            var success = await service.UpdateCharacterPotentialAsync(characterPotential);
            return success ? Results.Ok() : Results.BadRequest();
        });
    }
}
