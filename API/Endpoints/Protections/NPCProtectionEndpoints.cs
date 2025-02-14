using Business.Protections;
using Domain.Protections;

namespace API.Endpoints.Protections;

public static class NPCProtectionEndpoints
{
    public static void MapNPCProtectionEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/npc-protections").WithTags("NPCProtections");

        group.MapGet("/", async (INPCProtectionService service) =>
        {
            var npcProtections = await service.GetAllNPCProtectionsAsync();
            return Results.Ok(npcProtections);
        });

        group.MapGet("/{id:guid}", async (Guid id, INPCProtectionService service) =>
        {
            var npcProtection = await service.GetNPCProtectionByIdAsync(id);
            if (npcProtection == null)
            {
                return Results.NotFound();
            }
            return Results.Ok(npcProtection);
        });

        group.MapPost("/", async (NPCProtection npcProtection, INPCProtectionService service) =>
        {
            var createdNPCProtection = await service.CreateNPCProtectionAsync(npcProtection);
            return Results.Created($"/npc-protections/{createdNPCProtection.Id}", createdNPCProtection);
        });

        group.MapPut("/{id:guid}", async (Guid id, NPCProtection npcProtection, INPCProtectionService service) =>
        {
            var updatedNPCProtection = await service.UpdateNPCProtectionAsync(id, npcProtection);
            if (updatedNPCProtection == null)
            {
                return Results.NotFound();
            }
            return Results.Ok(updatedNPCProtection);
        });

        group.MapDelete("/{id:guid}", async (Guid id, INPCProtectionService service) =>
        {
            var success = await service.DeleteNPCProtectionAsync(id);
            if (!success)
            {
                return Results.NotFound();
            }
            return Results.NoContent();
        });
    }
}
