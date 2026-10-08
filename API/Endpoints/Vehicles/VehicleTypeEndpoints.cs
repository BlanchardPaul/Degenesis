using Business.Vehicles;
using Degenesis.Shared.DTOs;
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
            var result = await service.GetAllVehicleTypesAsync();
            return result.IsError ? Results.BadRequest(result) : Results.Ok(result);
        });

        group.MapGet("/{id:guid}", async (Guid id, IVehicleTypeService service) =>
        {
            var result = await service.GetVehicleTypeByIdAsync(id);
            return result.IsError ? Results.BadRequest(result) : Results.Ok(result);
        });

        group.MapPost("/", async (VehicleTypeCreateDto vehicleType, IVehicleTypeService service) =>
        {
            var result = await service.CreateVehicleTypeAsync(vehicleType);
            return result.IsError ? Results.BadRequest(result) : Results.Created();
        });

        group.MapPut("/", async (VehicleTypeDto vehicleType, IVehicleTypeService service) =>
        {
            var result = await service.UpdateVehicleTypeAsync(vehicleType);
            return result.IsError ? Results.BadRequest(result) : Results.Ok();
        });

        group.MapDelete("/{id:guid}", async (Guid id, IVehicleTypeService service) =>
        {
            var result = await service.DeleteVehicleTypeAsync(id);
            return result.IsError ? Results.BadRequest(result) : Results.NoContent();
        });
    }
}
