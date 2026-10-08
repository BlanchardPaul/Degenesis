using Business.Characters;
using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Characters.CRUD;

namespace API.Endpoints.Characters;

public static class RankEndpoints
{
    public static void MapRankEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/ranks").WithTags("Ranks").RequireAuthorization();

        group.MapGet("/", async (IRankService service) =>
        {
            var result = await service.GetAllRanksAsync();
            return result.IsError ? Results.BadRequest(result) : Results.Ok(result);
        });

        group.MapGet("/{id:guid}", async (Guid id, IRankService service) =>
        {
            var result = await service.GetRankByIdAsync(id);
            return result.IsError ? Results.BadRequest(result) : Results.Ok(result);
        });

        group.MapPost("/", async (RankCreateDto rank, IRankService service) =>
        {
            var result = await service.CreateRankAsync(rank);
            return result.IsError ? Results.BadRequest(result) : Results.Created();
        });

        group.MapPut("/", async (RankDto rank, IRankService service) =>
        {
            var result = await service.UpdateRankAsync(rank);
            return result.IsError ? Results.BadRequest(result) : Results.Ok(result);
        });

        group.MapDelete("/{id:guid}", async (Guid id, IRankService service) =>
        {
            var result = await service.DeleteRankAsync(id);
            return result.IsError ? Results.BadRequest(result) : Results.NoContent();
        });
    }
}
