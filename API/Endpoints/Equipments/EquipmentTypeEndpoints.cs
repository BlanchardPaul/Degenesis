using Business.Equipments;
using Degenesis.Shared.DTOs.Equipments;

namespace API.Endpoints.Equipments;

public static class EquipmentTypeEndpoints
{
    public static void MapEquipmentTypeEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/equipment-types").WithTags("EquipmentTypes");

        group.MapGet("/", async (IEquipmentTypeService service) =>
        {
            var equipmentTypes = await service.GetAllEquipmentTypesAsync();
            return Results.Ok(equipmentTypes);
        });

        group.MapGet("/{id:guid}", async (Guid id, IEquipmentTypeService service) =>
        {
            var equipmentType = await service.GetEquipmentTypeByIdAsync(id);
            return equipmentType is not null ? Results.Ok(equipmentType) : Results.NotFound();
        });

        group.MapPost("/", async (EquipmentTypeCreateDto equipmentType, IEquipmentTypeService service) =>
        {
            var created = await service.CreateEquipmentTypeAsync(equipmentType);
            return created is not null ? Results.Created() : Results.BadRequest();
        });

        group.MapPut("/", async (EquipmentTypeDto equipmentType, IEquipmentTypeService service) =>
        {
            var success = await service.UpdateEquipmentTypeAsync(equipmentType);
            return success ? Results.Ok() : Results.NotFound();
        });

        group.MapDelete("/{id:guid}", async (Guid id, IEquipmentTypeService service) =>
        {
            var success = await service.DeleteEquipmentTypeAsync(id);
            return success ? Results.NoContent() : Results.NotFound();
        });
    }
}
