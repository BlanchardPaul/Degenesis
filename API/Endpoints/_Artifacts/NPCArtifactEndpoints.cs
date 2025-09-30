//using Business._Artifacts;
//using Domain._Artifacts;

//namespace API.Endpoints._Artifacts;

//public static class NPCArtifactEndpoints
//{
//    public static void MapNPCArtifactEndpoints(this IEndpointRouteBuilder app)
//    {
//        var group = app.MapGroup("/npc-artifacts").WithTags("NPCArtifacts");

//        group.MapGet("/{id:guid}", async (Guid id, INPCArtifactService service) =>
//        {
//            var npcArtifact = await service.GetByIdAsync(id);
//            return npcArtifact is not null ? Results.Ok(npcArtifact) : Results.NotFound();
//        });

//        group.MapGet("/npc/{npcId:guid}", async (Guid npcId, INPCArtifactService service) =>
//        {
//            var artifacts = await service.GetByNPCIdAsync(npcId);
//            return artifacts.Any() ? Results.Ok(artifacts) : Results.NotFound();
//        });

//        group.MapPost("/", async (NPCArtifact npcArtifact, INPCArtifactService service) =>
//        {
//            var created = await service.CreateAsync(npcArtifact);
//            return Results.Created($"/npc-artifacts/{created.Id}", created);
//        });

//        group.MapPut("/{id:guid}", async (Guid id, NPCArtifact npcArtifact, INPCArtifactService service) =>
//        {
//            var success = await service.UpdateAsync(id, npcArtifact);
//            return success ? Results.NoContent() : Results.NotFound();
//        });

//        group.MapDelete("/{id:guid}", async (Guid id, INPCArtifactService service) =>
//        {
//            var success = await service.DeleteAsync(id);
//            return success ? Results.NoContent() : Results.NotFound();
//        });
//    }
//}