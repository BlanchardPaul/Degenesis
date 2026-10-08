using Business.Equipments;
using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Equipments;

namespace API.Endpoints.Equipments;

public static class EquipmentEndpoints
{
    public static void MapEquipmentEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/equipments").WithTags("Equipments");

        group.MapGet("/", async (IEquipmentService service) =>
        {
            var result = await service.GetAllEquipmentsAsync();
            return result.IsError ? Results.BadRequest(result) : Results.Ok(result);
        });

        group.MapGet("/{id:guid}", async (Guid id, IEquipmentService service) =>
        {
            var result = await service.GetEquipmentByIdAsync(id);
            return result.IsError ? Results.BadRequest(result) : Results.Ok(result);
        });

        group.MapPost("/", async (EquipmentCreateDto equipment, IEquipmentService service) =>
        {
            var result = await service.CreateEquipmentAsync(equipment);
            return result.IsError ? Results.BadRequest(result) : Results.Created();
        });

        group.MapPut("/", async (EquipmentDto equipment, IEquipmentService service) =>
        {
            var result = await service.UpdateEquipmentAsync(equipment);
            return result.IsError ? Results.BadRequest(result) : Results.Ok();
        });

        group.MapDelete("/{id:guid}", async (Guid id, IEquipmentService service) =>
        {
            var result = await service.DeleteEquipmentAsync(id);
            return result.IsError ? Results.BadRequest(result) : Results.NoContent();
        });
    }
}
