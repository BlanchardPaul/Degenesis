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
            return protectionQuality is not null ? Results.Ok(protectionQuality) : Results.NotFound();
        });

        group.MapPost("/", async (ProtectionQualityCreateDto protectionQuality, IProtectionQualityService service) =>
        {
            var created = await service.CreateProtectionQualityAsync(protectionQuality);
            return created is not null ? Results.Created() : Results.BadRequest();
        });

        group.MapPut("/", async (ProtectionQualityDto protectionQuality, IProtectionQualityService service) =>
        {
            var success = await service.UpdateProtectionQualityAsync(protectionQuality);
            return success ? Results.Ok() : Results.BadRequest();
        });

        group.MapDelete("/{id:guid}", async (Guid id, IProtectionQualityService service) =>
        {
            var success = await service.DeleteProtectionQualityAsync(id);
            return success ? Results.NoContent() : Results.NotFound();
        });
    }
}
