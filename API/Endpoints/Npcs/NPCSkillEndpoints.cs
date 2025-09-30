//using Business.NPCs;
//using Domain.NPCs;

//namespace API.Endpoints.Npcs;

//public static class NPCSkillEndpoints
//{
//    public static void MapNPCSkillEndpoints(this IEndpointRouteBuilder app)
//    {
//        var group = app.MapGroup("/npcskills").WithTags("NPC Skills");

//        group.MapGet("/", async (INPCSkillService service) =>
//        {
//            var npcSkills = await service.GetAllNPCSkillsAsync();
//            return Results.Ok(npcSkills);
//        });

//        group.MapGet("/{npcId:guid}/{skillId:guid}", async (Guid npcId, Guid skillId, INPCSkillService service) =>
//        {
//            var npcSkill = await service.GetNPCSkillByIdAsync(npcId, skillId);
//            return npcSkill is not null ? Results.Ok(npcSkill) : Results.NotFound();
//        });

//        group.MapPost("/", async (NPCSkill npcSkill, INPCSkillService service) =>
//        {
//            var createdNPCSkill = await service.CreateNPCSkillAsync(npcSkill);
//            return Results.Created($"/npcskills/{createdNPCSkill.NPCId}/{createdNPCSkill.SkillId}", createdNPCSkill);
//        });

//        group.MapPut("/{npcId:guid}/{skillId:guid}", async (Guid npcId, Guid skillId, NPCSkill npcSkill, INPCSkillService service) =>
//        {
//            var updatedNPCSkill = await service.UpdateNPCSkillAsync(npcId, skillId, npcSkill);
//            return updatedNPCSkill is not null ? Results.Ok(updatedNPCSkill) : Results.NotFound();
//        });

//        group.MapDelete("/{npcId:guid}/{skillId:guid}", async (Guid npcId, Guid skillId, INPCSkillService service) =>
//        {
//            var deleted = await service.DeleteNPCSkillAsync(npcId, skillId);
//            return deleted ? Results.NoContent() : Results.NotFound();
//        });
//    }
//}
