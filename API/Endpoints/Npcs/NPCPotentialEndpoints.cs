//using Business.NPCs;
//using Domain.NPCs;

//namespace API.Endpoints.Npcs;

//public static class NPCPotentialEndpoints
//{
//    public static void MapNPCPotentialEndpoints(this IEndpointRouteBuilder app)
//    {
//        var group = app.MapGroup("/npcpotentials").WithTags("NPC Potentials");

//        group.MapGet("/", async (INPCPotentialService service) =>
//        {
//            var npcPotentials = await service.GetAllNPCPotentialsAsync();
//            return Results.Ok(npcPotentials);
//        });

//        group.MapGet("/{npcId:guid}/{potentialId:guid}", async (Guid npcId, Guid potentialId, INPCPotentialService service) =>
//        {
//            var npcPotential = await service.GetNPCPotentialByIdAsync(npcId, potentialId);
//            return npcPotential is not null ? Results.Ok(npcPotential) : Results.NotFound();
//        });

//        group.MapPost("/", async (NPCPotential npcPotential, INPCPotentialService service) =>
//        {
//            var createdNPCPotential = await service.CreateNPCPotentialAsync(npcPotential);
//            return Results.Created($"/npcpotentials/{createdNPCPotential.NPCId}/{createdNPCPotential.PotentialId}", createdNPCPotential);
//        });

//        group.MapPut("/{npcId:guid}/{potentialId:guid}", async (Guid npcId, Guid potentialId, NPCPotential npcPotential, INPCPotentialService service) =>
//        {
//            var updatedNPCPotential = await service.UpdateNPCPotentialAsync(npcId, potentialId, npcPotential);
//            return updatedNPCPotential is not null ? Results.Ok(updatedNPCPotential) : Results.NotFound();
//        });

//        group.MapDelete("/{npcId:guid}/{potentialId:guid}", async (Guid npcId, Guid potentialId, INPCPotentialService service) =>
//        {
//            var deleted = await service.DeleteNPCPotentialAsync(npcId, potentialId);
//            return deleted ? Results.NoContent() : Results.NotFound();
//        });
//    }
//}
