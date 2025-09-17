using Business.Characters;
using Degenesis.Shared.DTOs.Characters;
using Domain.Characters;
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
            if (characters is null)
            {
                return Results.NotFound();
            }
            return Results.Ok(characters);
        });

        group.MapPost("/", async (CharacterCreateDto character, ICharacterService service, ClaimsPrincipal user) =>
        {
            var created = await service.CreateCharacterAsync(character, user?.Identity?.Name ?? string.Empty);
            if (created is null)
                return Results.BadRequest();
            return Results.Created();
        });

        group.MapPut("/{id:guid}", async (Guid id, Character character, ICharacterService service) =>
        {
            var success = await service.UpdateCharacterAsync(id, character);
            return success ? Results.NoContent() : Results.NotFound();
        });

        group.MapDelete("/{id:guid}", async (Guid id, ICharacterService service) =>
        {
            var success = await service.DeleteCharacterAsync(id);
            return success ? Results.NoContent() : Results.NotFound();
        });
    }
}