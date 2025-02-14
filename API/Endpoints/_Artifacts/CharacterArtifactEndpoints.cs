using Business.Artifacts;
using Domain.Artifacts;

namespace API.Endpoints.Artifacts;

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
    }
}