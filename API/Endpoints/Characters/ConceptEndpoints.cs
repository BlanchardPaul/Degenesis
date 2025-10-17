using Business.Characters;
using Degenesis.Shared.DTOs.Characters.CRUD;

namespace API.Endpoints.Characters;

public static class ConceptEndpoints
{
    public static void MapConceptEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/concepts").WithTags("Concepts").RequireAuthorization();

        group.MapGet("/", async (IConceptService service) =>
        {
            var concepts = await service.GetAllConceptsAsync();
            return Results.Ok(concepts);
        });

        group.MapGet("/{id:guid}", async (Guid id, IConceptService service) =>
        {
            var concept = await service.GetConceptByIdAsync(id);
            return concept is not null ? Results.Ok(concept) : Results.NotFound();
        });

        group.MapPost("/", async (ConceptCreateDto concept, IConceptService service) =>
        {
            var created = await service.CreateConceptAsync(concept);
            return created is not null ? Results.Ok() : Results.BadRequest();
        });

        group.MapPut("/", async (ConceptDto concept, IConceptService service) =>
        {
            var success = await service.UpdateConceptAsync(concept);
            return success ? Results.Ok() : Results.BadRequest();
        });

        group.MapDelete("/{id:guid}", async (Guid id, IConceptService service) =>
        {
            var success = await service.DeleteConceptAsync(id);
            return success ? Results.NoContent() : Results.NotFound();
        });
    }
}
