using Business._Artifacts;
using Degenesis.Shared.DTOs._Artifacts;

namespace API.Endpoints._Artifacts;

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

        group.MapPost("/", async (CharacterArtifactCreateDto artifact, ICharacterArtifactService service) =>
        {
            var created = await service.CreateAsync(artifact);
            return created is not null ? Results.Created() : Results.BadRequest();
        });

        group.MapPut("/", async (CharacterArtifactDto artifact, ICharacterArtifactService service) =>
        {
            var success = await service.UpdateAsync(artifact);
            return success ? Results.Ok() : Results.BadRequest();
        });

        group.MapDelete("/{id:guid}", async (Guid id, ICharacterArtifactService service) =>
        {
            var success = await service.DeleteAsync(id);
            return success ? Results.NoContent() : Results.NotFound();
        });
    }
}