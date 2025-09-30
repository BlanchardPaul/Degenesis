using Business.Equipments;
using Degenesis.Shared.DTOs.Equipments;

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
            return equipment is not null ? Results.Ok(equipment) : Results.NotFound();
        });

        group.MapPost("/", async (EquipmentCreateDto equipment, IEquipmentService service) =>
        {
            var created = await service.CreateEquipmentAsync(equipment);
            return created is not null ? Results.Created() : Results.BadRequest();
        });

        group.MapPut("/", async (EquipmentDto equipment, IEquipmentService service) =>
        {
            var success = await service.UpdateEquipmentAsync(equipment);
            return success ? Results.Ok() : Results.NotFound();
        });

        group.MapDelete("/{id:guid}", async (Guid id, IEquipmentService service) =>
        {
            var success = await service.DeleteEquipmentAsync(id);
            return success ? Results.NoContent() : Results.NotFound();
        });
    }
}
