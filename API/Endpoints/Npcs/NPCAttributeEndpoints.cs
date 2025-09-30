//using Business.NPCs;
//using Domain.NPCs;

//namespace API.Endpoints.Npcs;

//public static class NPCAttributeEndpoints
//{
//    public static void MapNPCAttributeEndpoints(this IEndpointRouteBuilder app)
//    {
//        var group = app.MapGroup("/npcattributes").WithTags("NPC Attributes");

//        group.MapGet("/", async (INPCAttributeService service) =>
//        {
//            var npcAttributes = await service.GetAllNPCAttributesAsync();
//            return Results.Ok(npcAttributes);
//        });

//        group.MapGet("/{npcId:guid}/{attributeId:guid}", async (Guid npcId, Guid attributeId, INPCAttributeService service) =>
//        {
//            var npcAttribute = await service.GetNPCAttributeByIdAsync(npcId, attributeId);
//            return npcAttribute is not null ? Results.Ok(npcAttribute) : Results.NotFound();
//        });

//        group.MapPost("/", async (NPCAttribute npcAttribute, INPCAttributeService service) =>
//        {
//            var createdNPCAttribute = await service.CreateNPCAttributeAsync(npcAttribute);
//            return Results.Created($"/npcattributes/{createdNPCAttribute.NPCId}/{createdNPCAttribute.AttributeId}", createdNPCAttribute);
//        });

//        group.MapPut("/{npcId:guid}/{attributeId:guid}", async (Guid npcId, Guid attributeId, NPCAttribute npcAttribute, INPCAttributeService service) =>
//        {
//            var updatedNPCAttribute = await service.UpdateNPCAttributeAsync(npcId, attributeId, npcAttribute);
//            return updatedNPCAttribute is not null ? Results.Ok(updatedNPCAttribute) : Results.NotFound();
//        });

//        group.MapDelete("/{npcId:guid}/{attributeId:guid}", async (Guid npcId, Guid attributeId, INPCAttributeService service) =>
//        {
//            var deleted = await service.DeleteNPCAttributeAsync(npcId, attributeId);
//            return deleted ? Results.NoContent() : Results.NotFound();
//        });
//    }
//}
