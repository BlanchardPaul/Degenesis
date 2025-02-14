using Business.Characters;
using Domain.Characters;

namespace API.Endpoints.Characters;

public static class ConceptEndpoints
{
    public static void MapConceptEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/concepts").WithTags("Concepts");

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

        group.MapPost("/", async (Concept concept, IConceptService service) =>
        {
            var created = await service.CreateConceptAsync(concept);
            return Results.Created($"/concepts/{created.Id}", created);
        });

        group.MapPut("/{id:guid}", async (Guid id, Concept concept, IConceptService service) =>
        {
            var success = await service.UpdateConceptAsync(id, concept);
            return success ? Results.NoContent() : Results.NotFound();
        });

        group.MapDelete("/{id:guid}", async (Guid id, IConceptService service) =>
        {
            var success = await service.DeleteConceptAsync(id);
            return success ? Results.NoContent() : Results.NotFound();
        });
    }
}
