using Business.NPCs;
using Degenesis.Shared.DTOs.NPCs;
using System.Security.Claims;

namespace API.Endpoints.Npcs;

public static class NPCEndpoints
{
    public static void MapNPCEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/npcs").WithTags("Room NPCs").RequireAuthorization();

        group.MapGet("/room/{roomId:guid}", async (Guid roomId, INPCService npcService) =>
        {
            var result = await npcService.GetAllByIdRoomAsync(roomId);
            return result.IsError ? Results.BadRequest(result) : Results.Ok(result);
        });

        group.MapGet("/{id:guid}", async (Guid id, INPCService npcService) =>
        {
            var result = await npcService.GetByIdAsync(id);
            return result.IsError ? Results.BadRequest(result) : Results.Ok(result);
        });

        group.MapPost("/{baseNPCId:guid}/room/{roomId:guid}", async (Guid baseNPCId, Guid roomId, INPCService npcService) =>
        {
            var result = await npcService.CreateAsync(baseNPCId, roomId);
            return result.IsError ? Results.BadRequest(result) : Results.Created();
        });

        group.MapPut("/", async (NPCDto npcDto, INPCService npcService) =>
        {
            var result = await npcService.UpdateAsync(npcDto);
            return result.IsError ? Results.BadRequest(result) : Results.Ok();
        });

        group.MapPost("/{id:guid}/avatar", async (Guid id, IFormFile file, INPCService npcService, ClaimsPrincipal user) =>
        {
            var result = await npcService.SetAvatarAsync(id, file, user.Identity?.Name ?? string.Empty);
            return result.IsError ? Results.BadRequest(result) : Results.Ok();
        }).DisableAntiforgery();

        group.MapDelete("/{id:guid}/avatar", async (Guid id, INPCService npcService, ClaimsPrincipal user) =>
        {
            var result = await npcService.RemoveAvatarAsync(id, user.Identity?.Name ?? string.Empty);
            return result.IsError ? Results.BadRequest(result) : Results.Ok();
        });

        group.MapDelete("/{id:guid}", async (Guid id, INPCService npcService) =>
        {
            var result = await npcService.DeleteAsync(id);
            return result.IsError ? Results.BadRequest(result) : Results.NoContent();
        });
    }
}
