using Business.Characters;
using Degenesis.Shared.DTOs.Characters.CRUD;
using Domain.Characters;

namespace API.Endpoints.Characters;

public static class CharacterBackgroundEndpoints
{
    public static void MapCharacterBackgroundEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/character-backgrounds").WithTags("Character Backgrounds");

        group.MapPut("/", async (CharacterBackgroundDto characterBackground, ICharacterBackgroundService service) =>
        {
            var result = await service.UpdateCharacterBackgroundAsync(characterBackground);
            return result.IsError ? Results.BadRequest(result) : Results.Ok();
        });
    }
}
