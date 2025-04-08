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
            if (equipment is null)
            {
                return Results.NotFound();
            }
            return Results.Ok(equipment);
        });

        group.MapPost("/", async (EquipmentCreateDto equipment, IEquipmentService service) =>
        {
            var created = await service.CreateEquipmentAsync(equipment);
            if (created is null)
                return Results.BadRequest();
            return Results.Created();
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
