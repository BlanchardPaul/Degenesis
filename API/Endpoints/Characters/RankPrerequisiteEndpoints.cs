using Business.Characters;
using Degenesis.Shared.DTOs.Characters;

namespace API.Endpoints.Characters;

public static class RankPrerequisiteEndpoints
{
    public static void MapRankPrerequisiteEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/rank-prerequisites").WithTags("RankPrerequisites").RequireAuthorization();

        group.MapGet("/", async (IRankPrerequisiteService service) =>
        {
            var rankPrerequisites = await service.GetAllRankPrerequisitesAsync();
            return Results.Ok(rankPrerequisites);
        });

        group.MapGet("/{id:guid}", async (Guid id, IRankPrerequisiteService service) =>
        {
            var rankPrerequisite = await service.GetRankPrerequisiteByIdAsync(id);
            return rankPrerequisite is not null ? Results.Ok(rankPrerequisite) : Results.NotFound();
        });

        group.MapPost("/", async (RankPrerequisiteCreateDto rankPrerequisite, IRankPrerequisiteService service) =>
        {
            var created = await service.CreateRankPrerequisiteAsync(rankPrerequisite);
            return created is not null ? Results.Created() : Results.BadRequest();
        });

        group.MapPut("/", async (RankPrerequisiteDto rankPrerequisite, IRankPrerequisiteService service) =>
        {
            var success = await service.UpdateRankPrerequisiteAsync(rankPrerequisite);
            return success ? Results.Ok() : Results.BadRequest();
        });

        group.MapDelete("/{id:guid}", async (Guid id, IRankPrerequisiteService service) =>
        {
            var success = await service.DeleteRankPrerequisiteAsync(id);
            return success ? Results.NoContent() : Results.NotFound();
        });
    }
}
