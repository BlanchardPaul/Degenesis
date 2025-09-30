using Business._Artifacts;
using Degenesis.Shared.DTOs._Artifacts;

namespace API.Endpoints._Artifacts;

public static class ArtifactEndpoints
{
    public static void MapArtifactEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/artifacts").WithTags("Artifacts").RequireAuthorization();

        group.MapGet("/", async (IArtifactService service) =>
        {
            return Results.Ok(await service.GetAllAsync());
        });

        group.MapGet("/{id:guid}", async (Guid id, IArtifactService service) =>
        {
            var artifact = await service.GetByIdAsync(id);
            return artifact is not null ? Results.Ok(artifact) : Results.NotFound();
        });

        group.MapPost("/", async (ArtifactCreateDto artifact, IArtifactService service) =>
        {
            var created = await service.CreateAsync(artifact);
            return created is not null ? Results.Created() : Results.BadRequest();
        });

        group.MapPut("/", async (ArtifactDto artifact, IArtifactService service) =>
        {
            var success = await service.UpdateAsync(artifact);
            return success ? Results.Ok() : Results.BadRequest();
        });

        group.MapDelete("/{id:guid}", async (Guid id, IArtifactService service) =>
        {
            var success = await service.DeleteAsync(id);
            return success ? Results.NoContent() : Results.NotFound();
        });
    }
}