using Business.Characters;
using Degenesis.Shared.DTOs.Characters;

namespace API.Endpoints.Characters;

public static class CharacterBackgroundEndpoints
{
    public static void MapCharacterBackgroundEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/character-backgrounds").WithTags("Character Backgrounds");

        group.MapPut("/", async (CharacterBackgroundDto characterBackground, ICharacterBackgroundService service) =>
        {
            var success = await service.UpdateCharacterBackgroundAsync(characterBackground);
            return success ? Results.Ok() : Results.BadRequest();
        });
    }
}
