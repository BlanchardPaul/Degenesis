using Business._Artifacts;
using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs._Artifacts;

namespace API.Endpoints._Artifacts;

public static class ArtifactEndpoints
{
    public static void MapArtifactEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/artifacts").WithTags("Artifacts").RequireAuthorization();

        group.MapGet("/", async (IArtifactService service) =>
        {
            var result = await service.GetAllAsync();
            return result.IsError ? Results.BadRequest(result) : Results.Ok(result);
        });

        group.MapGet("/{id:guid}", async (Guid id, IArtifactService service) =>
        {
            var result = await service.GetByIdAsync(id);
            return result.IsError ? Results.BadRequest(result) : Results.Ok(result);
        });

        group.MapPost("/", async (ArtifactCreateDto artifact, IArtifactService service) =>
        {
            var result = await service.CreateAsync(artifact);
            return result.IsError ? Results.BadRequest(result) : Results.Created();
        });

        group.MapPut("/", async (ArtifactDto artifact, IArtifactService service) =>
        {
            var result = await service.UpdateAsync(artifact);
            return result.IsError ? Results.BadRequest(result) : Results.Ok();
        });

        group.MapDelete("/{id:guid}", async (Guid id, IArtifactService service) =>
        {
            var result = await service.DeleteAsync(id);
            return result.IsError ? Results.BadRequest(result) : Results.NoContent();
        });
    }
}