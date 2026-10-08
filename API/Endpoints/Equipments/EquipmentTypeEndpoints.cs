using Business.Equipments;
using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Equipments;

namespace API.Endpoints.Equipments;

public static class EquipmentTypeEndpoints
{
    public static void MapEquipmentTypeEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/equipment-types").WithTags("EquipmentTypes");

        group.MapGet("/", async (IEquipmentTypeService service) =>
        {
            var result = await service.GetAllEquipmentTypesAsync();
            return result.IsError ? Results.BadRequest(result) : Results.Ok(result);
        });

        group.MapGet("/{id:guid}", async (Guid id, IEquipmentTypeService service) =>
        {
            var result = await service.GetEquipmentTypeByIdAsync(id);
            return result.IsError ? Results.BadRequest(result) : Results.Ok(result);
        });

        group.MapPost("/", async (EquipmentTypeCreateDto equipmentType, IEquipmentTypeService service) =>
        {
            var result = await service.CreateEquipmentTypeAsync(equipmentType);
            return result.IsError ? Results.BadRequest(result) : Results.Created();
        });

        group.MapPut("/", async (EquipmentTypeDto equipmentType, IEquipmentTypeService service) =>
        {
            var result = await service.UpdateEquipmentTypeAsync(equipmentType);
            return result.IsError ? Results.BadRequest(result) : Results.Ok();
        });

        group.MapDelete("/{id:guid}", async (Guid id, IEquipmentTypeService service) =>
        {
            var result = await service.DeleteEquipmentTypeAsync(id);
            return result.IsError ? Results.BadRequest(result) : Results.NoContent();
        });
    }
}
