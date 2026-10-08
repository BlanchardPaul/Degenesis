using Business.Rooms;
using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Rooms;
using System.Security.Claims;

namespace API.Endpoints.Rooms;

public static class RoomEndpoints
{
    public static void MapRoomEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/rooms").WithTags("Rooms").RequireAuthorization();


        group.MapGet("/", async (IRoomService service, ClaimsPrincipal user) =>
        {
            var result = await service.GetAllAsync(user?.Identity?.Name ?? string.Empty);
            return result.IsError ? Results.BadRequest(result) : Results.Ok(result);
        });

        group.MapGet("/{id:guid}", async (Guid id, IRoomService service) =>
        {
            var result = await service.GetByIdAsync(id);
            return result.IsError ? Results.BadRequest(result) : Results.Ok(result);
        });

        group.MapPost("/", async (RoomCreateDto room, IRoomService service, ClaimsPrincipal user) =>
        {
            var result = await service.CreateAsync(room, user?.Identity?.Name ?? string.Empty);
            return result.IsError ? Results.BadRequest(result) : Results.Created();
        });

        group.MapPut("/", async (RoomDto room, IRoomService service) =>
        {
            var result = await service.UpdateAsync(room);
            return result.IsError ? Results.BadRequest(result) : Results.Ok();
        });

        group.MapDelete("/{id:guid}", async (Guid id, IRoomService service) =>
        {
            var result = await service.DeleteRoomAsync(id);
            return result.IsError ? Results.BadRequest(result) : Results.NoContent();
        });

        group.MapPost("/invite", async (InvitationDto invitationDto, IRoomService service) =>
        {
            var result = await service.InviteUser(invitationDto);
            return result.IsError ? Results.BadRequest(result) : Results.Ok();
        });

        group.MapGet("/acceptinvite/{idRoom:guid}", async (Guid idRoom, IRoomService service, ClaimsPrincipal user) =>
        {
            var result = await service.AccepteInvite(idRoom, user?.Identity?.Name ?? string.Empty);
            return result.IsError ? Results.BadRequest(result) : Results.Ok();
        });

        group.MapGet("/declineinvite/{idRoom:guid}", async (Guid idRoom, IRoomService service, ClaimsPrincipal user) =>
        {
            var result = await service.DeclineInvite(idRoom, user?.Identity?.Name ?? string.Empty);
            return result.IsError ? Results.BadRequest(result) : Results.Ok();
        });
    }
}
