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
            var characterArtifact = await service.GetByIdAsync(id);
            return characterArtifact is not null ? Results.Ok(characterArtifact) : Results.NotFound();
        });

        group.MapGet("/character/{characterId:guid}", async (Guid characterId, ICharacterArtifactService service) =>
        {
            var artifacts = await service.GetByCharacterIdAsync(characterId);
            return artifacts.Any() ? Results.Ok(artifacts) : Results.NotFound();
        });

        group.MapPost("/", async (CharacterArtifactCreateDto characterArtifact, ICharacterArtifactService service) =>
        {
            var created = await service.CreateAsync(characterArtifact);
            return created is not null ? Results.Created() : Results.BadRequest();
        });

        group.MapPut("/", async (CharacterArtifactDto characterArtifact, ICharacterArtifactService service) =>
        {
            var success = await service.UpdateAsync(characterArtifact);
            return success ? Results.Ok() : Results.BadRequest();
        });

        group.MapDelete("/{id:guid}", async (Guid id, ICharacterArtifactService service) =>
        {
            var success = await service.DeleteAsync(id);
            return success ? Results.NoContent() : Results.NotFound();
        });
    }
}
