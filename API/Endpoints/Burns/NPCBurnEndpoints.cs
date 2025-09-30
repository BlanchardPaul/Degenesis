//using Business.Burns;
//using Domain.Burns;

//namespace API.Endpoints.Burns;

//public static class NPCBurnEndpoints
//{
//    public static void MapNPCBurnEndpoints(this IEndpointRouteBuilder app)
//    {
//        var group = app.MapGroup("/npc-burns").WithTags("NPCBurns");

//        group.MapGet("/", async (INPCBurnService service) =>
//        {
//            var npcBurns = await service.GetAllNPCBurnsAsync();
//            return Results.Ok(npcBurns);
//        });

//        group.MapGet("/{npcId:guid}/{burnId:guid}", async (Guid npcId, Guid burnId, INPCBurnService service) =>
//        {
//            var npcBurn = await service.GetNPCBurnByIdAsync(npcId, burnId);
//            if (npcBurn is null)
//            {
//                return Results.NotFound();
//            }
//            return Results.Ok(npcBurn);
//        });

//        group.MapPost("/", async (NPCBurn npcBurn, INPCBurnService service) =>
//        {
//            var createdNPCBurn = await service.CreateNPCBurnAsync(npcBurn);
//            return Results.Created($"/npc-burns/{createdNPCBurn.NPCId}/{createdNPCBurn.BurnId}", createdNPCBurn);
//        });

//        group.MapPut("/{npcId:guid}/{burnId:guid}", async (Guid npcId, Guid burnId, NPCBurn npcBurn, INPCBurnService service) =>
//        {
//            var updatedNPCBurn = await service.UpdateNPCBurnAsync(npcId, burnId, npcBurn);
//            if (updatedNPCBurn is null)
//            {
//                return Results.NotFound();
//            }
//            return Results.Ok(updatedNPCBurn);
//        });

//        group.MapDelete("/{npcId:guid}/{burnId:guid}", async (Guid npcId, Guid burnId, INPCBurnService service) =>
//        {
//            var success = await service.DeleteNPCBurnAsync(npcId, burnId);
//            if (!success)
//            {
//                return Results.NotFound();
//            }
//            return Results.NoContent();
//        });
//    }
//}
