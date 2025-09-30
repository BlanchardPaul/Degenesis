using Business.Characters;
using Degenesis.Shared.DTOs.Characters;
using System.Security.Claims;

namespace API.Endpoints.Characters;

public static class CharacterEndpoints
{
    public static void MapCharacterEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/characters").WithTags("Characters");

        group.MapGet("/{id:guid}", async (Guid id, ICharacterService service) =>
        {
            var character = await service.GetCharacterByIdAsync(id);
            return character is not null ? Results.Ok(character) : Results.NotFound();
        });

        group.MapGet("/", async (ICharacterService service) =>
        {
            var characters = await service.GetAllCharactersAsync();
            return characters is not null ? Results.Ok(characters) : Results.NotFound();
        });

        group.MapPost("/", async (CharacterCreateDto character, ICharacterService service, ClaimsPrincipal user) =>
        {
            var created = await service.CreateCharacterAsync(character, user?.Identity?.Name ?? string.Empty);
            return character is not null ? Results.Created() : Results.BadRequest();
        });

        group.MapPut("/", async (CharacterDto character, ICharacterService service) =>
        {
            var success = await service.UpdateCharacterAsync(character);
            return success ? Results.Ok() : Results.BadRequest();
        });

        group.MapDelete("/{id:guid}", async (Guid id, ICharacterService service) =>
        {
            var success = await service.DeleteCharacterAsync(id);
            return success ? Results.NoContent() : Results.NotFound();
        });
    }
}