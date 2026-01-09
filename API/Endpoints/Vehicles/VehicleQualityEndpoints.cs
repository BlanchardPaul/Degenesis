using Business.Vehicles;
using Degenesis.Shared.DTOs.Vehicles;

namespace API.Endpoints.Vehicles;

public static class VehicleQualityEndpoints
{
    public static void MapVehicleQualityEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/vehicle-qualities").WithTags("VehicleQualities").RequireAuthorization();

        group.MapGet("/{id}", async (IVehicleQualityService service, Guid id) =>
        {
            var vehicleQuality = await service.GetVehicleQualityByIdAsync(id);
            return vehicleQuality is not null ? Results.Ok(vehicleQuality) : Results.NotFound();
        });

        group.MapGet("/", async (IVehicleQualityService service) =>
        {
            var vehicleQualities = await service.GetAllVehicleQualitiesAsync();
            return Results.Ok(vehicleQualities);
        });

        group.MapPost("/", async (IVehicleQualityService service, VehicleQualityCreateDto vehicleQuality) =>
        {
            var created = await service.CreateVehicleQualityAsync(vehicleQuality);
            return created is not null ? Results.Created() : Results.BadRequest();
        });

        group.MapPut("/", async (IVehicleQualityService service, VehicleQualityDto vehicleQuality) =>
        {
            var success = await service.UpdateVehicleQualityAsync(vehicleQuality);
            return success ? Results.Ok() : Results.BadRequest();
        });

        group.MapDelete("/{id}", async (IVehicleQualityService service, Guid id) =>
        {
            var success = await service.DeleteVehicleQualityAsync(id);
            return success ? Results.NoContent() : Results.NotFound();
        });
    }
}
