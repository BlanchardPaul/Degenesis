using Business.Characters;
using Degenesis.Shared.DTOs.Characters;

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

        group.MapPost("/", async (RankCreateDto rank, IRankService service) =>
        {
            var createdRank = await service.CreateRankAsync(rank);
            return createdRank is not null
               ? Results.Created($"/ranks/{createdRank.Id}", createdRank)
               : Results.BadRequest();
        });

        group.MapPut("/", async (RankDto rank, IRankService service) =>
        {
            var success = await service.UpdateRankAsync(rank);
            return success ? Results.NoContent() : Results.NotFound();
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
