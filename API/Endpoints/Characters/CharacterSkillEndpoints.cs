using Business.Characters;
using Domain.Characters;

namespace API.Endpoints.Characters;

public static class CharacterSkillEndpoints
{
    public static void MapCharacterSkillEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/character-skills").WithTags("CharacterSkills");

        // GET /character-skills/{characterId}/{skillId}
        group.MapGet("/{characterId:guid}/{skillId:guid}", async (Guid characterId, Guid skillId, ICharacterSkillService service) =>
        {
            var characterSkill = await service.GetCharacterSkillByIdAsync(characterId, skillId);
            return characterSkill is not null ? Results.Ok(characterSkill) : Results.NotFound();
        });

        // GET /character-skills/character/{characterId}
        group.MapGet("/character/{characterId:guid}", async (Guid characterId, ICharacterSkillService service) =>
        {
            var characterSkills = await service.GetCharacterSkillsByCharacterIdAsync(characterId);
            return Results.Ok(characterSkills);
        });

        // GET /character-skills/skill/{skillId}
        group.MapGet("/skill/{skillId:guid}", async (Guid skillId, ICharacterSkillService service) =>
        {
            var characterSkills = await service.GetCharacterSkillsBySkillIdAsync(skillId);
            return Results.Ok(characterSkills);
        });

        // POST /character-skills
        group.MapPost("/", async (CharacterSkill characterSkill, ICharacterSkillService service) =>
        {
            var created = await service.CreateCharacterSkillAsync(characterSkill);
            return Results.Created($"/character-skills/{created.CharacterId}/{created.SkillId}", created);
        });

        // PUT /character-skills/{characterId}/{skillId}
        group.MapPut("/{characterId:guid}/{skillId:guid}", async (Guid characterId, Guid skillId, CharacterSkill characterSkill, ICharacterSkillService service) =>
        {
            var success = await service.UpdateCharacterSkillAsync(characterId, skillId, characterSkill);
            return success ? Results.NoContent() : Results.NotFound();
        });

        // DELETE /character-skills/{characterId}/{skillId}
        group.MapDelete("/{characterId:guid}/{skillId:guid}", async (Guid characterId, Guid skillId, ICharacterSkillService service) =>
        {
            var success = await service.DeleteCharacterSkillAsync(characterId, skillId);
            return success ? Results.NoContent() : Results.NotFound();
        });
    }
}
