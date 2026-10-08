using Business.Characters;
using Degenesis.Shared.DTOs.Characters.CRUD;

namespace API.Endpoints.Characters;

public static class RankPrerequisiteEndpoints
{
    public static void MapRankPrerequisiteEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/rank-prerequisites").WithTags("RankPrerequisites").RequireAuthorization();

        group.MapGet("/", async (IRankPrerequisiteService service) =>
        {
            var result = await service.GetAllRankPrerequisitesAsync();
            return result.IsError ? Results.BadRequest(result) : Results.Ok(result);
        });

        group.MapGet("/{id:guid}", async (Guid id, IRankPrerequisiteService service) =>
        {
            var result = await service.GetRankPrerequisiteByIdAsync(id);
            return result.IsError ? Results.BadRequest(result) : Results.Ok(result);
        });

        group.MapPost("/", async (RankPrerequisiteCreateDto rankPrerequisite, IRankPrerequisiteService service) =>
        {
            var result = await service.CreateRankPrerequisiteAsync(rankPrerequisite);
            return result.IsError ? Results.BadRequest(result) : Results.Created();
        });

        group.MapPut("/", async (RankPrerequisiteDto rankPrerequisite, IRankPrerequisiteService service) =>
        {
            var result = await service.UpdateRankPrerequisiteAsync(rankPrerequisite);
            return result.IsError ? Results.BadRequest(result) : Results.Ok();
        });

        group.MapDelete("/{id:guid}", async (Guid id, IRankPrerequisiteService service) =>
        {
            var result = await service.DeleteRankPrerequisiteAsync(id);
            return result.IsError ? Results.BadRequest(result) : Results.NoContent();
        });
    }
}
