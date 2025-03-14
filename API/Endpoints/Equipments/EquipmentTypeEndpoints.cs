using Business.Equipments;
using Degenesis.Shared.DTOs.Equipments;
using Domain.Equipments;

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
            if (equipmentType == null)
            {
                return Results.NotFound();
            }
            return Results.Ok(equipmentType);
        });

        group.MapPost("/", async (EquipmentTypeCreateDto equipmentType, IEquipmentTypeService service) =>
        {
            var createdEquipmentType = await service.CreateEquipmentTypeAsync(equipmentType);
            return Results.Created($"/equipment-types/{createdEquipmentType.Id}", createdEquipmentType);
        });

        group.MapPut("/", async (EquipmentTypeDto equipmentType, IEquipmentTypeService service) =>
        {
            var updatedEquipmentType = await service.UpdateEquipmentTypeAsync(equipmentType);
            return updatedEquipmentType ? Results.NoContent() : Results.NotFound();
        });

        group.MapDelete("/{id:guid}", async (Guid id, IEquipmentTypeService service) =>
        {
            var success = await service.DeleteEquipmentTypeAsync(id);
            if (!success)
            {
                return Results.NotFound();
            }
            return Results.NoContent();
        });
    }
}
