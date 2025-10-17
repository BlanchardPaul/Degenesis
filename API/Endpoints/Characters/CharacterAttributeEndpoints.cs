using Business.Characters;
using Degenesis.Shared.DTOs.Characters.CRUD;

namespace API.Endpoints.Characters;

public static class CharacterAttributeEndpoints
{
    public static void MapCharacterAttributeEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/character-attributes").WithTags("Character Attributes");

        group.MapPut("/", async (CharacterAttributeDto characterAttribute, ICharacterAttributeService service) =>
        {
            var success = await service.UpdateCharacterAttributeAsync(characterAttribute);
            return success ? Results.Ok() : Results.BadRequest();
        });
    }
}
