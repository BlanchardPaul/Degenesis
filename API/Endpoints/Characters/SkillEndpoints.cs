using Business.Characters;
using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Characters.CRUD;

namespace API.Endpoints.Characters;

public static class SkillEndpoints
{
    public static void MapSkillEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/skills").WithTags("Skills").RequireAuthorization();

        group.MapGet("/", async (ISkillService service) =>
        {
            var result = await service.GetAllSkillsAsync();
            return result.IsError ? Results.BadRequest(result) : Results.Ok(result);
        });

        group.MapGet("/{id:guid}", async (Guid id, ISkillService service) =>
        {
            var result = await service.GetSkillByIdAsync(id);
            return result.IsError ? Results.BadRequest(result) : Results.Ok(result);
        });

        group.MapPost("/", async (SkillCreateDto skill, ISkillService service) =>
        {
            var result = await service.CreateSkillAsync(skill);
            return result.IsError ? Results.BadRequest(result) : Results.Created();
        });

        group.MapPut("/", async (SkillDto skill, ISkillService service) =>
        {
            var result = await service.UpdateSkillAsync(skill);
            return result.IsError ? Results.BadRequest(result) : Results.Ok();
        });

        group.MapDelete("/{id:guid}", async (Guid id, ISkillService service) =>
        {
            var result = await service.DeleteSkillAsync(id);
            return result.IsError ? Results.BadRequest(result) : Results.NoContent();
        });
    }
}
