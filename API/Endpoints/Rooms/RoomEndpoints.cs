using Business.Rooms;
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
            var rooms = await service.GetAllAsync(user?.Identity?.Name ?? string.Empty);
            return Results.Ok(rooms);
        });

        group.MapGet("/{id:guid}", async (Guid id, IRoomService service) =>
        {
            var room = await service.GetByIdAsync(id);
            return room is not null ? Results.Ok(room) : Results.NotFound();
        });

        group.MapPost("/", async (RoomCreateDto room, IRoomService service, ClaimsPrincipal user) =>
        {
            var createdRoomId = await service.CreateAsync(room, user?.Identity?.Name ?? string.Empty);
            return createdRoomId is not null ? Results.Created() : Results.BadRequest();
        });

        group.MapPut("/", async (RoomDto room, IRoomService service) =>
        {
            var success = await service.UpdateAsync(room);
            return success ? Results.Ok() : Results.BadRequest();
        });

        group.MapDelete("/{id:guid}", async (Guid id, IRoomService service) =>
        {
            var success = await service.DeleteRoomAsync(id);
            return success ? Results.NoContent() : Results.NotFound();
        });

        group.MapPost("/invite", async (InvitationDto invitationDto, IRoomService service) =>
        {
            var success = await service.InviteUser(invitationDto);
            return success ? Results.Ok() : Results.BadRequest();
        });

        group.MapGet("/acceptinvite/{idRoom:guid}", async (Guid idRoom, IRoomService service, ClaimsPrincipal user) =>
        {
            var success = await service.AccepteInvite(idRoom, user?.Identity?.Name ?? string.Empty);
            return success ? Results.Ok() : Results.BadRequest();
        });

        group.MapGet("/declineinvite/{idRoom:guid}", async (Guid idRoom, IRoomService service, ClaimsPrincipal user) =>
        {
            var success = await service.DeclineInvite(idRoom, user?.Identity?.Name ?? string.Empty);
            return success ? Results.Ok() : Results.BadRequest();
        });
    }
}
