using Business.Vehicles;
using Degenesis.Shared.DTOs.Vehicles;
using Domain.Vehicles;

namespace API.Endpoints.Vehicles;

public static class VehicleEndpoints
{
    public static void MapVehicleEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/vehicles").WithTags("Vehicles");

        group.MapGet("/", async (IVehicleService service) =>
        {
            var vehicles = await service.GetAllVehiclesAsync();
            return Results.Ok(vehicles);
        });

        group.MapGet("/{id:guid}", async (Guid id, IVehicleService service) =>
        {
            var vehicle = await service.GetVehicleByIdAsync(id);
            if (vehicle == null)
            {
                return Results.NotFound();
            }
            return Results.Ok(vehicle);
        });

        group.MapPost("/", async (VehicleCreateDto vehicle, IVehicleService service) =>
        {
            var createdVehicle = await service.CreateVehicleAsync(vehicle);
            return Results.Created($"/vehicles/{createdVehicle.Id}", createdVehicle);
        });

        group.MapPut("/", async (VehicleDto vehicle, IVehicleService service) =>
        {
            var updatedVehicle = await service.UpdateVehicleAsync(vehicle);
            return updatedVehicle ? Results.NoContent() : Results.NotFound();
        });

        group.MapDelete("/{id:guid}", async (Guid id, IVehicleService service) =>
        {
            var success = await service.DeleteVehicleAsync(id);
            if (!success)
            {
                return Results.NotFound();
            }
            return Results.NoContent();
        });
    }
}
