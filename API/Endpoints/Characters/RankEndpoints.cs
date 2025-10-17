using Business.Characters;
using Degenesis.Shared.DTOs.Characters.CRUD;

namespace API.Endpoints.Characters;

public static class RankEndpoints
{
    public static void MapRankEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/ranks").WithTags("Ranks").RequireAuthorization();

        group.MapGet("/", async (IRankService service) =>
        {
            var ranks = await service.GetAllRanksAsync();
            return Results.Ok(ranks);
        });

        group.MapGet("/{id:guid}", async (Guid id, IRankService service) =>
        {
            var rank = await service.GetRankByIdAsync(id);
            return rank is not null ? Results.Ok(rank) : Results.NotFound();
        });

        group.MapPost("/", async (RankCreateDto rank, IRankService service) =>
        {
            var created = await service.CreateRankAsync(rank);
            return created is not null ? Results.Created() : Results.BadRequest();
        });

        group.MapPut("/", async (RankDto rank, IRankService service) =>
        {
            var success = await service.UpdateRankAsync(rank);
            return success ? Results.Ok() : Results.BadRequest();
        });

        group.MapDelete("/{id:guid}", async (Guid id, IRankService service) =>
        {
            var success = await service.DeleteRankAsync(id);
            return success ? Results.NoContent() : Results.NotFound();
        });
    }
}
