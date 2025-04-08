using Business.Vehicles;
using Degenesis.Shared.DTOs.Vehicles;

namespace API.Endpoints.Vehicles;

public static class VehicleEndpoints
{
    public static void MapVehicleEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/vehicles").WithTags("Vehicles").RequireAuthorization();

        group.MapGet("/", async (IVehicleService service) =>
        {
            var vehicles = await service.GetAllVehiclesAsync();
            return Results.Ok(vehicles);
        });

        group.MapGet("/{id:guid}", async (Guid id, IVehicleService service) =>
        {
            var vehicle = await service.GetVehicleByIdAsync(id);
            if (vehicle is null)
            {
                return Results.NotFound();
            }
            return Results.Ok(vehicle);
        });

        group.MapPost("/", async (VehicleCreateDto vehicle, IVehicleService service) =>
        {
            var created = await service.CreateVehicleAsync(vehicle);
            if (created is null)
                return Results.BadRequest();
            return Results.Created();
        });

        group.MapPut("/", async (VehicleDto vehicle, IVehicleService service) =>
        {
            var success = await service.UpdateVehicleAsync(vehicle);
            return success ? Results.Ok() : Results.NotFound();
        });

        group.MapDelete("/{id:guid}", async (Guid id, IVehicleService service) =>
        {
            var success = await service.DeleteVehicleAsync(id);
            return success ? Results.NoContent() : Results.NotFound();
        });
    }
}
