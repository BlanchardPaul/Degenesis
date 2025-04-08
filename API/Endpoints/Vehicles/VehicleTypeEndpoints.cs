using Business.Vehicles;
using Degenesis.Shared.DTOs.Vehicles;
using Domain.Vehicles;
using Microsoft.AspNetCore.Http.HttpResults;
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

        group.MapPost("/", async (VehicleTypeCreateDto vehicleType, IVehicleTypeService service) =>
        {
            var created = await service.CreateVehicleTypeAsync(vehicleType);
            if (created is null)
                return Results.BadRequest();
            return Results.Created();
        });

        group.MapPut("/", async (VehicleTypeDto vehicleType, IVehicleTypeService service) =>
        {
            var success = await service.UpdateVehicleTypeAsync(vehicleType);
            return success ? Results.Ok() : Results.NotFound();
        });

        group.MapDelete("/{id:guid}", async (Guid id, IVehicleTypeService service) =>
        {
            var success = await service.DeleteVehicleTypeAsync(id);
            return success ? Results.NoContent() : Results.NotFound();
        });
    }
}
