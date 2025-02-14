using Business.Protections;
using Domain.Protections;

namespace API.Endpoints.Protections;

public static class ProtectionEndpoints
{
    public static void MapProtectionEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/protections").WithTags("Protections");

        group.MapGet("/", async (IProtectionService service) =>
        {
            var protections = await service.GetAllProtectionsAsync();
            return Results.Ok(protections);
        });

        group.MapGet("/{id:guid}", async (Guid id, IProtectionService service) =>
        {
            var protection = await service.GetProtectionByIdAsync(id);
            if (protection == null)
            {
                return Results.NotFound();
            }
            return Results.Ok(protection);
        });

        group.MapPost("/", async (Protection protection, IProtectionService service) =>
        {
            var createdProtection = await service.CreateProtectionAsync(protection);
            return Results.Created($"/protections/{createdProtection.Id}", createdProtection);
        });

        group.MapPut("/{id:guid}", async (Guid id, Protection protection, IProtectionService service) =>
        {
            var updatedProtection = await service.UpdateProtectionAsync(id, protection);
            if (updatedProtection == null)
            {
                return Results.NotFound();
            }
            return Results.Ok(updatedProtection);
        });

        group.MapDelete("/{id:guid}", async (Guid id, IProtectionService service) =>
        {
            var success = await service.DeleteProtectionAsync(id);
            if (!success)
            {
                return Results.NotFound();
            }
            return Results.NoContent();
        });
    }
}
