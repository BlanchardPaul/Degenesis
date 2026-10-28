using Business.NPCs;
using Degenesis.Shared.DTOs;

namespace API.Endpoints.Npcs;

public static class BaseNPCEndpoints
{
    public static void MapBaseNPCEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/base-npcs").RequireAuthorization();

        group.MapGet("/", async (IBaseNPCService service) =>
        {
            var result = await service.GetAllAsync();
            return result.IsError ? Results.BadRequest(result) : Results.Ok(result);
        });

        group.MapPost("/", async (BaseNPCCreateDto npc, IBaseNPCService baseNPCService) =>
        {
            var result = await baseNPCService.CreateAsync(npc);
            return result.IsError ? Results.BadRequest(result) : Results.Created();
        });

        group.MapPut("/", async (BaseNPCDto npc, IBaseNPCService baseNPCService) =>
        {
            var result = await baseNPCService.BasicInfosUpdateAsync(npc);
            return result.IsError ? Results.BadRequest(result) : Results.Ok();
        });

        group.MapPut("/stats", async (BaseNPCDto npc, IBaseNPCService baseNPCService) =>
        {
            var result = await baseNPCService.StatInfosUpdateAsync(npc);
            return result.IsError ? Results.BadRequest(result) : Results.Ok();
        });

        group.MapDelete("/{id:guid}", async (Guid id, IBaseNPCService service) =>
        {
            var result = await service.DeleteAsync(id);
            return result.IsError ? Results.BadRequest(result) : Results.Ok();
        });
    }
}
