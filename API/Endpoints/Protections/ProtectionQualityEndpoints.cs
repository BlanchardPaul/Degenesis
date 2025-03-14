using Business.Protections;
using Degenesis.Shared.DTOs.Protections;

namespace API.Endpoints.Protections;

public static class ProtectionQualityEndpoints
{
    public static void MapProtectionQualityEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/protection-qualities").WithTags("ProtectionQualities");

        group.MapGet("/", async (IProtectionQualityService service) =>
        {
            var protectionQualities = await service.GetAllProtectionQualitiesAsync();
            return Results.Ok(protectionQualities);
        });

        group.MapGet("/{id:guid}", async (Guid id, IProtectionQualityService service) =>
        {
            var protectionQuality = await service.GetProtectionQualityByIdAsync(id);
            if (protectionQuality == null)
            {
                return Results.NotFound();
            }
            return Results.Ok(protectionQuality);
        });

        group.MapPost("/", async (ProtectionQualityCreateDto protectionQuality, IProtectionQualityService service) =>
        {
            var createdProtectionQuality = await service.CreateProtectionQualityAsync(protectionQuality);
            return Results.Created($"/protection-qualities/{createdProtectionQuality.Id}", createdProtectionQuality);
        });

        group.MapPut("/", async (ProtectionQualityDto protectionQuality, IProtectionQualityService service) =>
        {
            var updatedProtectionQuality = await service.UpdateProtectionQualityAsync(protectionQuality);
            return updatedProtectionQuality ? Results.NoContent() : Results.NotFound();
        });

        group.MapDelete("/{id:guid}", async (Guid id, IProtectionQualityService service) =>
        {
            var success = await service.DeleteProtectionQualityAsync(id);
            if (!success)
            {
                return Results.NotFound();
            }
            return Results.NoContent();
        });
    }
}
