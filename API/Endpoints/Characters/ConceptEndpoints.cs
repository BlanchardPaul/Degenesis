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
            var result = await service.GetAllConceptsAsync();
            return result.IsError ? Results.BadRequest(result) : Results.Ok(result);
        });

        group.MapGet("/{id:guid}", async (Guid id, IConceptService service) =>
        {
            var result = await service.GetConceptByIdAsync(id);
            return result.IsError ? Results.BadRequest(result) : Results.Ok(result);
        });

        group.MapPost("/", async (ConceptCreateDto concept, IConceptService service) =>
        {
            var result = await service.CreateConceptAsync(concept);
            return result.IsError ? Results.BadRequest(result) : Results.Created();
        });

        group.MapPut("/", async (ConceptDto concept, IConceptService service) =>
        {
            var result = await service.UpdateConceptAsync(concept);
            return result.IsError ? Results.BadRequest(result) : Results.Ok();
        });

        group.MapDelete("/{id:guid}", async (Guid id, IConceptService service) =>
        {
            var result = await service.DeleteConceptAsync(id);
            return result.IsError ? Results.BadRequest(result) : Results.NoContent();
        });
    }
}
