using Business.Vehicles;
using Domain.Vehicles;
using Microsoft.AspNetCore.Mvc;

namespace API.Endpoints.Vehicles;

public static class VehicleTypeEndpoints
{
    public static void MapVehicleTypeEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/vehicle-types").WithTags("Vehicle Types");

        group.MapGet("/", async (IVehicleTypeService service) =>
        {
            var vehicleTypes = await service.GetAllVehicleTypesAsync();
            return Results.Ok(vehicleTypes);
        });

        group.MapGet("/{id:guid}", async (Guid id, IVehicleTypeService service) =>
        {
            var vehicleType = await service.GetVehicleTypeByIdAsync(id);
            return vehicleType is not null ? Results.Ok(vehicleType) : Results.NotFound();
        });

        group.MapPost("/", async ([FromBody] VehicleType vehicleType, IVehicleTypeService service) =>
        {
            var createdVehicleType = await service.CreateVehicleTypeAsync(vehicleType);
            return Results.Created($"/vehicle-types/{createdVehicleType.Id}", createdVehicleType);
        });

        group.MapPut("/{id:guid}", async (Guid id, [FromBody] VehicleType vehicleType, IVehicleTypeService service) =>
        {
            var updatedVehicleType = await service.UpdateVehicleTypeAsync(id, vehicleType);
            return updatedVehicleType is not null ? Results.Ok(updatedVehicleType) : Results.NotFound();
        });

        group.MapDelete("/{id:guid}", async (Guid id, IVehicleTypeService service) =>
        {
            var success = await service.DeleteVehicleTypeAsync(id);
            return success ? Results.NoContent() : Results.NotFound();
        });
    }
}
