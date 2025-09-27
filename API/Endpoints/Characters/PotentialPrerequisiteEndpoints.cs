using Business.Characters;
using Degenesis.Shared.DTOs.Characters;

namespace API.Endpoints.Characters;

public static class PotentialPrerequisiteEndpoints
{
    public static void MapPotentialPrerequisiteEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/potential-prerequisites")
                       .WithTags("PotentialPrerequisites")
                       .RequireAuthorization();

        group.MapGet("/", async (IPotentialPrerequisiteService service) =>
        {
            var prerequisites = await service.GetAllPotentialPrerequisitesAsync();
            return Results.Ok(prerequisites);
        });

        group.MapGet("/{id:guid}", async (Guid id, IPotentialPrerequisiteService service) =>
        {
            var prerequisite = await service.GetPotentialPrerequisiteByIdAsync(id);
            if (prerequisite is null)
            {
                return Results.NotFound();
            }
            return Results.Ok(prerequisite);
        });

        group.MapPost("/", async (PotentialPrerequisiteCreateDto prerequisite, IPotentialPrerequisiteService service) =>
        {
            var created = await service.CreatePotentialPrerequisiteAsync(prerequisite);
            if (created is null)
                return Results.BadRequest();
            return Results.Created($"/potential-prerequisites/{created.Id}", created);
        });

        group.MapPut("/", async (PotentialPrerequisiteDto prerequisite, IPotentialPrerequisiteService service) =>
        {
            var success = await service.UpdatePotentialPrerequisiteAsync(prerequisite);
            return success ? Results.Ok() : Results.NotFound();
        });

        group.MapDelete("/{id:guid}", async (Guid id, IPotentialPrerequisiteService service) =>
        {
            var success = await service.DeletePotentialPrerequisiteAsync(id);
            return success ? Results.NoContent() : Results.NotFound();
        });
    }
}
