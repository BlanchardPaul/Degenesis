using Business.Characters;
using Domain.Characters;

namespace API.Endpoints.Characters;

public static class RankEndpoints
{
    public static void MapRankEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/ranks").WithTags("Ranks");

        group.MapGet("/", async (IRankService service) =>
        {
            var ranks = await service.GetAllRanksAsync();
            return Results.Ok(ranks);
        });

        group.MapGet("/{id:guid}", async (Guid id, IRankService service) =>
        {
            var rank = await service.GetRankByIdAsync(id);
            if (rank == null)
            {
                return Results.NotFound();
            }
            return Results.Ok(rank);
        });

        group.MapPost("/", async (Rank rank, IRankService service) =>
        {
            var createdRank = await service.CreateRankAsync(rank);
            return Results.Created($"/ranks/{createdRank.Id}", createdRank);
        });

        group.MapPut("/{id:guid}", async (Guid id, Rank rank, IRankService service) =>
        {
            var updatedRank = await service.UpdateRankAsync(id, rank);
            if (updatedRank == null)
            {
                return Results.NotFound();
            }
            return Results.Ok(updatedRank);
        });

        group.MapDelete("/{id:guid}", async (Guid id, IRankService service) =>
        {
            var success = await service.DeleteRankAsync(id);
            if (!success)
            {
                return Results.NotFound();
            }
            return Results.NoContent();
        });
    }
}
