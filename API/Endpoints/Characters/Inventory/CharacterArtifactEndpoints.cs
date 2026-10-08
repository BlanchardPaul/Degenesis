using Business.Characters.Inventory;
using Degenesis.Shared.DTOs.Characters.CRUD.Inventory;

namespace API.Endpoints.Characters.Inventory;

public static class CharacterArtifactEndpoints
{
    public static void MapCharacterArtifactEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/character-artifacts").WithTags("CharacterArtifacts");

        group.MapGet("/{id:guid}", async (Guid id, ICharacterArtifactService service) =>
        {
            var result = await service.GetByIdAsync(id);
            return result.IsError ? Results.BadRequest(result) : Results.Ok(result);
        });

        group.MapGet("/character/{characterId:guid}", async (Guid characterId, ICharacterArtifactService service) =>
        {
            var result = await service.GetByCharacterIdAsync(characterId);
            return result.IsError ? Results.BadRequest(result) : Results.Ok(result);
        });

        group.MapPost("/", async (CharacterArtifactCreateDto characterArtifact, ICharacterArtifactService service) =>
        {
            var result = await service.CreateAsync(characterArtifact);
            return result.IsError ? Results.BadRequest(result) : Results.Created();
        });

        group.MapPut("/", async (CharacterArtifactDto characterArtifact, ICharacterArtifactService service) =>
        {
            var result = await service.UpdateAsync(characterArtifact);
            return result.IsError ? Results.BadRequest(result) : Results.Ok();
        });

        group.MapDelete("/{id:guid}", async (Guid id, ICharacterArtifactService service) =>
        {
            var result = await service.DeleteAsync(id);
            return result.IsError ? Results.BadRequest(result) : Results.NoContent();
        });
    }
}
