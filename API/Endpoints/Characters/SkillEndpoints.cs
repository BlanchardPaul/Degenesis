using Business.Characters;
using Degenesis.Shared.DTOs.Characters;
using Domain.Characters;

namespace API.Endpoints.Characters;

public static class SkillEndpoints
{
    public static void MapSkillEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/skills").WithTags("Skills").RequireAuthorization();

        group.MapGet("/", async (ISkillService service) =>
        {
            var skills = await service.GetAllSkillsAsync();
            return Results.Ok(skills);
        });

        group.MapGet("/{id:guid}", async (Guid id, ISkillService service) =>
        {
            var skill = await service.GetSkillByIdAsync(id);
            if (skill is null)
            {
                return Results.NotFound();
            }
            return Results.Ok(skill);
        });

        group.MapPost("/", async (SkillCreateDto skill, ISkillService service) =>
        {
            var created = await service.CreateSkillAsync(skill);
            if (created is null)
                return Results.BadRequest();
            return Results.Created();
        });

        group.MapPut("/", async (Skill skill, ISkillService service) =>
        {
            var success = await service.UpdateSkillAsync(skill);
            return success ? Results.Ok() : Results.NotFound();
        });

        group.MapDelete("/{id:guid}", async (Guid id, ISkillService service) =>
        {
            var success = await service.DeleteSkillAsync(id);
            return success ? Results.NoContent() : Results.NotFound();
        });
    }
}
