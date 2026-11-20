using Business.Rooms;
using System.Security.Claims;

namespace API.Endpoints.Rooms;

public static class RoomMapEndpoints
{
    public static void MapRoomMapEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/room-maps").WithTags("Room Maps").RequireAuthorization();

        group.MapGet("/room/{roomId:guid}", async (Guid roomId, IRoomMapService service, ClaimsPrincipal user) =>
        {
            var result = await service.GetByRoomAsync(roomId, user.Identity?.Name ?? string.Empty);
            return result.IsError ? Results.BadRequest(result) : Results.Ok(result);
        });

        group.MapPost("/room/{roomId:guid}", async (Guid roomId, IFormFile file, IRoomMapService service, ClaimsPrincipal user) =>
        {
            var result = await service.UploadAsync(roomId, file, user.Identity?.Name ?? string.Empty);
            return result.IsError ? Results.BadRequest(result) : Results.Ok(result);
        }).DisableAntiforgery();

        group.MapGet("/{mapId:guid}/tokens", async (Guid mapId, IRoomMapService service, ClaimsPrincipal user) =>
        {
            var result = await service.GetTokensAsync(mapId, user.Identity?.Name ?? string.Empty);
            return result.IsError ? Results.BadRequest(result) : Results.Ok(result);
        });

        group.MapPut("/room/{roomId:guid}/select/{mapId:guid}", async (Guid roomId, Guid mapId,
            IRoomMapService service, ClaimsPrincipal user) =>
        {
            var result = await service.SelectMapAsync(roomId, mapId, user.Identity?.Name ?? string.Empty);
            return result.IsError ? Results.BadRequest(result) : Results.Ok(result);
        });

        group.MapPut("/{mapId:guid}/tokens", async (Guid mapId, List<Degenesis.Shared.DTOs.Rooms.RoomMapTokenDto> tokens,
            IRoomMapService service, ClaimsPrincipal user) =>
        {
            var result = await service.SaveTokensAsync(mapId, tokens, user.Identity?.Name ?? string.Empty);
            return result.IsError ? Results.BadRequest(result) : Results.Ok(result);
        });

        group.MapPut("/{mapId:guid}/grid", async (Guid mapId, int gridCellSize,
            IRoomMapService service, ClaimsPrincipal user) =>
        {
            var result = await service.SaveGridCellSizeAsync(mapId, gridCellSize, user.Identity?.Name ?? string.Empty);
            return result.IsError ? Results.BadRequest(result) : Results.Ok(result);
        });

        group.MapDelete("/{mapId:guid}", async (Guid mapId, IRoomMapService service, ClaimsPrincipal user) =>
        {
            var result = await service.DeleteAsync(mapId, user.Identity?.Name ?? string.Empty);
            return result.IsError ? Results.BadRequest(result) : Results.Ok(result);
        });
    }
}
