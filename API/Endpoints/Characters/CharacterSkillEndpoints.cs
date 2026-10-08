using Business.Characters;
using Degenesis.Shared.DTOs.Characters.CRUD;

namespace API.Endpoints.Characters;

public static class CharacterSkillEndpoints
{
    public static void MapCharacterSkillEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/character-skills").WithTags("CharacterSkills");

        group.MapPut("/", async (CharacterSkillDto characterSkill, ICharacterSkillService service) =>
        {
            var result = await service.UpdateCharacterSkillAsync(characterSkill);
            return result.IsError ? Results.BadRequest(result) : Results.Created();
        });
    }
}
