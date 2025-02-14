using Business.NPCs;
using Domain.NPCs;

namespace API.Endpoints.Npcs;

public static class NPCEndpoints
{
    public static void MapNPCEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/npcs").WithTags("NPCs");

        group.MapGet("/", async (INPCService service) =>
        {
            var npcs = await service.GetAllNPCsAsync();
            return Results.Ok(npcs);
        });

        group.MapGet("/{id:guid}", async (Guid id, INPCService service) =>
        {
            var npc = await service.GetNPCByIdAsync(id);
            return npc is not null ? Results.Ok(npc) : Results.NotFound();
        });

        group.MapPost("/", async (NPC npc, INPCService service) =>
        {
            var createdNpc = await service.CreateNPCAsync(npc);
            return Results.Created($"/npcs/{createdNpc.Id}", createdNpc);
        });

        group.MapPut("/{id:guid}", async (Guid id, NPC npc, INPCService service) =>
        {
            var updatedNpc = await service.UpdateNPCAsync(id, npc);
            return updatedNpc is not null ? Results.Ok(updatedNpc) : Results.NotFound();
        });

        group.MapDelete("/{id:guid}", async (Guid id, INPCService service) =>
        {
            var deleted = await service.DeleteNPCAsync(id);
            return deleted ? Results.NoContent() : Results.NotFound();
        });
    }
}
