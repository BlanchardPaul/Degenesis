using Business.Equipments;
using Domain.Equipments;

namespace API.Endpoints.Equipments;

public static class NPCEquipmentEndpoints
{
    public static void MapNPCEquipmentEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/npc-equipments").WithTags("NPCEquipments");

        group.MapGet("/", async (INPCEquipmentService service) =>
        {
            var npcEquipments = await service.GetAllNPCEquipmentsAsync();
            return Results.Ok(npcEquipments);
        });

        group.MapGet("/{id:guid}", async (Guid id, INPCEquipmentService service) =>
        {
            var npcEquipment = await service.GetNPCEquipmentByIdAsync(id);
            if (npcEquipment == null)
            {
                return Results.NotFound();
            }
            return Results.Ok(npcEquipment);
        });

        group.MapPost("/", async (NPCEquipment npcEquipment, INPCEquipmentService service) =>
        {
            var createdNPCEquipment = await service.CreateNPCEquipmentAsync(npcEquipment);
            return Results.Created($"/npc-equipments/{createdNPCEquipment.Id}", createdNPCEquipment);
        });

        group.MapPut("/{id:guid}", async (Guid id, NPCEquipment npcEquipment, INPCEquipmentService service) =>
        {
            var updatedNPCEquipment = await service.UpdateNPCEquipmentAsync(id, npcEquipment);
            if (updatedNPCEquipment == null)
            {
                return Results.NotFound();
            }
            return Results.Ok(updatedNPCEquipment);
        });

        group.MapDelete("/{id:guid}", async (Guid id, INPCEquipmentService service) =>
        {
            var success = await service.DeleteNPCEquipmentAsync(id);
            if (!success)
            {
                return Results.NotFound();
            }
            return Results.NoContent();
        });
    }
}
