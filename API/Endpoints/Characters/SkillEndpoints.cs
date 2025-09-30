using Business.Characters;
using Degenesis.Shared.DTOs.Characters;

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
            return skill is not null ? Results.Ok(skill) : Results.NotFound();
        });

        group.MapPost("/", async (SkillCreateDto skill, ISkillService service) =>
        {
            var created = await service.CreateSkillAsync(skill);
            return created is not null ? Results.Created() : Results.BadRequest();
        });

        group.MapPut("/", async (SkillDto skill, ISkillService service) =>
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
