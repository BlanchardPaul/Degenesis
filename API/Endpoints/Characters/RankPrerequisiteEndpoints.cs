using Business.Characters;
using Degenesis.Shared.DTOs.Characters;

namespace API.Endpoints.Characters;

public static class RankPrerequisiteEndpoints
{
    public static void MapRankPrerequisiteEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/rank-prerequisites").WithTags("RankPrerequisites");

        group.MapGet("/", async (IRankPrerequisiteService service) =>
        {
            var rankPrerequisites = await service.GetAllRankPrerequisitesAsync();
            return Results.Ok(rankPrerequisites);
        });

        group.MapGet("/{id:guid}", async (Guid id, IRankPrerequisiteService service) =>
        {
            var rankPrerequisite = await service.GetRankPrerequisiteByIdAsync(id);
            if (rankPrerequisite is null)
            {
                return Results.NotFound();
            }
            return Results.Ok(rankPrerequisite);
        });

        group.MapPost("/", async (RankPrerequisiteCreateDto rankPrerequisite, IRankPrerequisiteService service) =>
        {
            var createdRankPrerequisite = await service.CreateRankPrerequisiteAsync(rankPrerequisite);
            return createdRankPrerequisite is not null
               ? Results.Created($"/rank-prerequisites/{createdRankPrerequisite.Id}", createdRankPrerequisite)
               : Results.BadRequest();
        });

        group.MapPut("/", async (RankPrerequisiteDto rankPrerequisite, IRankPrerequisiteService service) =>
        {
            var success = await service.UpdateRankPrerequisiteAsync(rankPrerequisite);
            return success ? Results.NoContent() : Results.NotFound();
        });

        group.MapDelete("/{id:guid}", async (Guid id, IRankPrerequisiteService service) =>
        {
            var success = await service.DeleteRankPrerequisiteAsync(id);
            if (!success)
            {
                return Results.NotFound();
            }
            return Results.NoContent();
        });
    }
}
