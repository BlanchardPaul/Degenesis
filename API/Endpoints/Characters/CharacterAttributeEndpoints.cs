using Business.Characters;
using Degenesis.Shared.DTOs.Characters.CRUD;
using Domain.Characters;

namespace API.Endpoints.Characters;

public static class CharacterAttributeEndpoints
{
    public static void MapCharacterAttributeEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/character-attributes").WithTags("Character Attributes");

        group.MapPut("/", async (CharacterAttributeDto characterAttribute, ICharacterAttributeService service) =>
        {
            var result = await service.UpdateCharacterAttributeAsync(characterAttribute);
            return result.IsError ? Results.BadRequest(result) : Results.Ok();
        });
    }
}
