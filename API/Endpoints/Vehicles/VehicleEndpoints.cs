using Business.Vehicles;
using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Vehicles;

namespace API.Endpoints.Vehicles;

public static class VehicleEndpoints
{
    public static void MapVehicleEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/vehicles").WithTags("Vehicles").RequireAuthorization();

        group.MapGet("/", async (IVehicleService service) =>
        {
            var result = await service.GetAllVehiclesAsync();
            return result.IsError ? Results.BadRequest(result) : Results.Ok(result);
        });

        group.MapGet("/{id:guid}", async (Guid id, IVehicleService service) =>
        {
            var result = await service.GetVehicleByIdAsync(id);
            return result.IsError ? Results.BadRequest(result) : Results.Ok(result);
        });

        group.MapPost("/", async (VehicleCreateDto vehicle, IVehicleService service) =>
        {
            var result = await service.CreateVehicleAsync(vehicle);
            return result.IsError ? Results.BadRequest(result) : Results.Created();
        });

        group.MapPut("/", async (VehicleDto vehicle, IVehicleService service) =>
        {
            var result = await service.UpdateVehicleAsync(vehicle);
            return result.IsError ? Results.BadRequest(result) : Results.Ok();
        });

        group.MapDelete("/{id:guid}", async (Guid id, IVehicleService service) =>
        {
            var result = await service.DeleteVehicleAsync(id);
            return result.IsError ? Results.BadRequest(result) : Results.NoContent();
        });
    }
}
