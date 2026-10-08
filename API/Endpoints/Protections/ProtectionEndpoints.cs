using Business.Protections;
using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Protections;

namespace API.Endpoints.Protections;

public static class ProtectionEndpoints
{
    public static void MapProtectionEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/protections").WithTags("Protections").RequireAuthorization();

        group.MapGet("/", async (IProtectionService service) =>
        {
            var result = await service.GetAllProtectionsAsync();
            return result.IsError ? Results.BadRequest(result) : Results.Ok(result);
        });

        group.MapGet("/{id:guid}", async (Guid id, IProtectionService service) =>
        {
            var result = await service.GetProtectionByIdAsync(id);
            return result.IsError ? Results.BadRequest(result) : Results.Ok(result);
        });

        group.MapPost("/", async (ProtectionCreateDto protection, IProtectionService service) =>
        {
            var result = await service.CreateProtectionAsync(protection);
            return result.IsError ? Results.BadRequest(result) : Results.Created();
        });

        group.MapPut("/", async (ProtectionDto protection, IProtectionService service) =>
        {
            var result = await service.UpdateProtectionAsync(protection);
            return result.IsError ? Results.BadRequest(result) : Results.Ok();
        });

        group.MapDelete("/{id:guid}", async (Guid id, IProtectionService service) =>
        {
            var result = await service.DeleteProtectionAsync(id);
            return result.IsError ? Results.BadRequest(result) : Results.NoContent();
        });
    }
}
