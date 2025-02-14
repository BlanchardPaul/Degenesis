using Business.Vehicles;
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

        group.MapPost("/", async (Vehicle vehicle, IVehicleService service) =>
        {
            var createdVehicle = await service.CreateVehicleAsync(vehicle);
            return Results.Created($"/vehicles/{createdVehicle.Id}", createdVehicle);
        });

        group.MapPut("/{id:guid}", async (Guid id, Vehicle vehicle, IVehicleService service) =>
        {
            var updatedVehicle = await service.UpdateVehicleAsync(id, vehicle);
            if (updatedVehicle == null)
            {
                return Results.NotFound();
            }
            return Results.Ok(updatedVehicle);
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
