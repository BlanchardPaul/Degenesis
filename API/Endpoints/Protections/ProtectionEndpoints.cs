using Business.Protections;
using Degenesis.Shared.DTOs.Protections;

namespace API.Endpoints.Protections;

public static class ProtectionEndpoints
{
    public static void MapProtectionEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/protections").WithTags("Protections").RequireAuthorization();

        group.MapGet("/", async (IProtectionService service) =>
        {
            var protections = await service.GetAllProtectionsAsync();
            return Results.Ok(protections);
        });

        group.MapGet("/{id:guid}", async (Guid id, IProtectionService service) =>
        {
            var protection = await service.GetProtectionByIdAsync(id);
            if (protection is null)
            {
                return Results.NotFound();
            }
            return Results.Ok(protection);
        });

        group.MapPost("/", async (ProtectionCreateDto protection, IProtectionService service) =>
        {
            var created = await service.CreateProtectionAsync(protection);
            if (created is null)
                return Results.BadRequest();
            return Results.Created();
        });

        group.MapPut("/", async (ProtectionDto protection, IProtectionService service) =>
        {
            var success = await service.UpdateProtectionAsync(protection);
            return success ? Results.Ok() : Results.NotFound();
        });

        group.MapDelete("/{id:guid}", async (Guid id, IProtectionService service) =>
        {
            var success = await service.DeleteProtectionAsync(id);
            return success ? Results.NoContent() : Results.NotFound();
        });
    }
}
