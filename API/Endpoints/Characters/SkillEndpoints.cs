using Business.Characters;
using Domain.Characters;

namespace API.Endpoints.Characters;

public static class SkillEndpoints
{
    public static void MapSkillEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/skills").WithTags("Skills");

        group.MapGet("/", async (ISkillService service) =>
        {
            var skills = await service.GetAllSkillsAsync();
            return Results.Ok(skills);
        });

        group.MapGet("/{id:guid}", async (Guid id, ISkillService service) =>
        {
            var skill = await service.GetSkillByIdAsync(id);
            if (skill == null)
            {
                return Results.NotFound();
            }
            return Results.Ok(skill);
        });

        group.MapPost("/", async (Skill skill, ISkillService service) =>
        {
            var createdSkill = await service.CreateSkillAsync(skill);
            return Results.Created($"/skills/{createdSkill.Id}", createdSkill);
        });

        group.MapPut("/{id:guid}", async (Guid id, Skill skill, ISkillService service) =>
        {
            var updatedSkill = await service.UpdateSkillAsync(id, skill);
            if (updatedSkill == null)
            {
                return Results.NotFound();
            }
            return Results.Ok(updatedSkill);
        });

        group.MapDelete("/{id:guid}", async (Guid id, ISkillService service) =>
        {
            var success = await service.DeleteSkillAsync(id);
            if (!success)
            {
                return Results.NotFound();
            }
            return Results.NoContent();
        });
    }
}
