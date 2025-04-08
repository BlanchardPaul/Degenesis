using Business.Protections;
using Degenesis.Shared.DTOs.Protections;

namespace API.Endpoints.Protections;

public static class ProtectionQualityEndpoints
{
    public static void MapProtectionQualityEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/protection-qualities").WithTags("ProtectionQualities").RequireAuthorization();

        group.MapGet("/", async (IProtectionQualityService service) =>
        {
            var protectionQualities = await service.GetAllProtectionQualitiesAsync();
            return Results.Ok(protectionQualities);
        });

        group.MapGet("/{id:guid}", async (Guid id, IProtectionQualityService service) =>
        {
            var protectionQuality = await service.GetProtectionQualityByIdAsync(id);
            if (protectionQuality is null)
            {
                return Results.NotFound();
            }
            return Results.Ok(protectionQuality);
        });

        group.MapPost("/", async (ProtectionQualityCreateDto protectionQuality, IProtectionQualityService service) =>
        {
            var created = await service.CreateProtectionQualityAsync(protectionQuality);
            if (created is null)
                return Results.BadRequest();
            return Results.Created();
        });

        group.MapPut("/", async (ProtectionQualityDto protectionQuality, IProtectionQualityService service) =>
        {
            var success = await service.UpdateProtectionQualityAsync(protectionQuality);
            return success ? Results.Ok() : Results.NotFound();
        });

        group.MapDelete("/{id:guid}", async (Guid id, IProtectionQualityService service) =>
        {
            var success = await service.DeleteProtectionQualityAsync(id);
            return success ? Results.NoContent() : Results.NotFound();
        });
    }
}
