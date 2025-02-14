using Business.Characters;
using Domain.Characters;

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
            if (rankPrerequisite == null)
            {
                return Results.NotFound();
            }
            return Results.Ok(rankPrerequisite);
        });

        group.MapPost("/", async (RankPrerequisite rankPrerequisite, IRankPrerequisiteService service) =>
        {
            var createdRankPrerequisite = await service.CreateRankPrerequisiteAsync(rankPrerequisite);
            return Results.Created($"/rank-prerequisites/{createdRankPrerequisite.Id}", createdRankPrerequisite);
        });

        group.MapPut("/{id:guid}", async (Guid id, RankPrerequisite rankPrerequisite, IRankPrerequisiteService service) =>
        {
            var updatedRankPrerequisite = await service.UpdateRankPrerequisiteAsync(id, rankPrerequisite);
            if (updatedRankPrerequisite == null)
            {
                return Results.NotFound();
            }
            return Results.Ok(updatedRankPrerequisite);
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
