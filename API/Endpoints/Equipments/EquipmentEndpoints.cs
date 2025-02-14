using Business.Equipments;
using Domain.Equipments;

namespace API.Endpoints.Equipments;

public static class EquipmentEndpoints
{
    public static void MapEquipmentEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/equipments").WithTags("Equipments");

        group.MapGet("/", async (IEquipmentService service) =>
        {
            var equipments = await service.GetAllEquipmentsAsync();
            return Results.Ok(equipments);
        });

        group.MapGet("/{id:guid}", async (Guid id, IEquipmentService service) =>
        {
            var equipment = await service.GetEquipmentByIdAsync(id);
            if (equipment == null)
            {
                return Results.NotFound();
            }
            return Results.Ok(equipment);
        });

        group.MapPost("/", async (Equipment equipment, IEquipmentService service) =>
        {
            var createdEquipment = await service.CreateEquipmentAsync(equipment);
            return Results.Created($"/equipments/{createdEquipment.Id}", createdEquipment);
        });

        group.MapPut("/{id:guid}", async (Guid id, Equipment equipment, IEquipmentService service) =>
        {
            var updatedEquipment = await service.UpdateEquipmentAsync(id, equipment);
            if (updatedEquipment == null)
            {
                return Results.NotFound();
            }
            return Results.Ok(updatedEquipment);
        });

        group.MapDelete("/{id:guid}", async (Guid id, IEquipmentService service) =>
        {
            var success = await service.DeleteEquipmentAsync(id);
            if (!success)
            {
                return Results.NotFound();
            }
            return Results.NoContent();
        });
    }
}
