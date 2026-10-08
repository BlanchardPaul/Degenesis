using Business.Characters;
using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Characters.CRUD;

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
            var result = await service.GetAllPotentialPrerequisitesAsync();
            return result.IsError ? Results.BadRequest(result) : Results.Ok(result);
        });

        group.MapGet("/{id:guid}", async (Guid id, IPotentialPrerequisiteService service) =>
        {
            var result = await service.GetPotentialPrerequisiteByIdAsync(id);
            return result.IsError ? Results.BadRequest(result) : Results.Ok(result);
        });

        group.MapPost("/", async (PotentialPrerequisiteCreateDto prerequisite, IPotentialPrerequisiteService service) =>
        {
            var result = await service.CreatePotentialPrerequisiteAsync(prerequisite);
            return result.IsError ? Results.BadRequest(result) : Results.Created();
        });

        group.MapPut("/", async (PotentialPrerequisiteDto prerequisite, IPotentialPrerequisiteService service) =>
        {
            var result = await service.UpdatePotentialPrerequisiteAsync(prerequisite);
            return result.IsError ? Results.BadRequest(result) : Results.Ok();
        });

        group.MapDelete("/{id:guid}", async (Guid id, IPotentialPrerequisiteService service) =>
        {
            var result = await service.DeletePotentialPrerequisiteAsync(id);
            return result.IsError ? Results.BadRequest(result) : Results.NoContent();
        });
    }
}
