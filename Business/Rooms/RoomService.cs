using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs.Rooms;
using Domain._Artifacts;
using Domain.Rooms;
using Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Business.Rooms;

public interface IRoomService
{
    Task<IEnumerable<RoomDisplayDto>> GetAllAsync(string userName);
    Task<RoomDisplayDto?> GetByIdAsync(Guid id);
    Task<Guid?> CreateAsync(RoomCreateDto roomCreate, string userName);
    Task<bool> UpdateAsync(RoomDto room);
    Task<bool> DeleteRoomAsync(Guid id);
    Task<bool> InviteUser(InvitationDto invitationDto);
    Task<bool> AccepteInvite(Guid idRoom, string userName);
    Task<bool> DeclineInvite(Guid idRoom, string userName);
}

public class RoomService : IRoomService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly UserManager<ApplicationUser> _userManager;

    public RoomService(ApplicationDbContext context, IMapper mapper, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _mapper = mapper;
        _userManager = userManager;
    }


    public async Task<IEnumerable<RoomDisplayDto>> GetAllAsync(string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);
        if (user is null)
            return [];
        var rooms = await _context.Rooms
            .Include(r => r.Characters)
            .Include(r => r.UserRooms)
            .ThenInclude(ur => ur.ApplicationUser)
            .Where(r => r.UserRooms.Any(ur => ur.IdApplicationUser == user.Id))
            .ToListAsync();

        List<RoomDisplayDto> displayRooms = [];
        foreach (var room in rooms) {
            displayRooms.Add(new RoomDisplayDto {
                Id = room.Id,
                Name = room.Name,
                Description = room.Description,
                GMName = room.UserRooms.First(ur => ur.IsGM).ApplicationUser.UserName ?? string.Empty,
                Players = room.UserRooms.Where(ur => !ur.IsGM && ur.InvitationAccepted).Select(ur => ur.ApplicationUser.UserName ?? string.Empty).ToList(),
                Pendings = room.UserRooms.Where(ur => !ur.IsGM && !ur.InvitationAccepted).Select(ur => ur.ApplicationUser.UserName ?? string.Empty).ToList(),
                IsPendingForCurrentUser = !room.UserRooms.First(ur => ur.IdApplicationUser == user.Id).InvitationAccepted,
                IsCurrentUserGM = room.UserRooms.First(ur => ur.IdApplicationUser == user.Id).IsGM,
                HasCharacter = room.Characters.Exists(c => c.IdApplicationUser == user.Id)
            });
        }
        return displayRooms;
    }

    public async Task<RoomDisplayDto?> GetByIdAsync(Guid id)
    {
        try
        {
            var dbRoom = await _context.Rooms
                .Include(r => r.UserRooms)
                .ThenInclude(ur => ur.ApplicationUser)
                .FirstOrDefaultAsync(r => r.Id == id);
            if (dbRoom is null)
                return null;

            return new RoomDisplayDto
            {
                Id = dbRoom.Id,
                Name = dbRoom.Name,
                Description = dbRoom.Description,
                GMName = dbRoom.UserRooms.First(ur => ur.IsGM).ApplicationUser.UserName ?? string.Empty,
                Players = dbRoom.UserRooms.Where(ur => !ur.IsGM && ur.InvitationAccepted).Select(ur => ur.ApplicationUser.UserName ?? string.Empty).ToList(),
                Pendings = dbRoom.UserRooms.Where(ur => !ur.IsGM && !ur.InvitationAccepted).Select(ur => ur.ApplicationUser.UserName ?? string.Empty).ToList()
            };
        }
        catch(Exception)
        {
            return null;
        }

    }

    public async Task<Guid?> CreateAsync(RoomCreateDto roomCreate, string userName)
    {
        try
        {
            var creator = await _userManager.FindByNameAsync(userName);
            if (creator is null)
                return null;

            var dbUser = _context.Users.Find(creator.Id);
            if (dbUser is null)
                return null;

            var room = _mapper.Map<Room>(roomCreate);

            room.UserRooms = new List<UserRoom>
            {
                new UserRoom
                {
                    IdApplicationUser = creator.Id,
                    IsGM = true,
                    InvitationAccepted = true,
                    ApplicationUser = dbUser
                }
            };

            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();

            return room.Id;
        }
        catch(Exception)
        {
            return null;
        }
    }

    public async Task<bool> UpdateAsync(RoomDto room)
    {
        var existing = await _context.Rooms.IgnoreAutoIncludes().FirstOrDefaultAsync(r => r.Id == room.Id);
        if (existing is null) return false;

        _mapper.Map(room, existing);

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteRoomAsync(Guid id)
    {
        var room = await _context.Rooms.FindAsync(id);
        if (room is null) return false;

        _context.Rooms.Remove(room);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> InviteUser(InvitationDto invitationDto)
    {
        try
        {
            var dbRoom = await _context.Rooms.FindAsync(invitationDto.IdRoom);
            if (dbRoom is null) return false;

            var user = await _userManager.FindByNameAsync(invitationDto.UserName);
            if (user is null) return false;

            //Check if user alreasy invited
            var existingUserRoom = await _context.UserRooms.FirstOrDefaultAsync(ur => ur.IdRoom == invitationDto.IdRoom && ur.IdApplicationUser == user.Id);
            if (existingUserRoom is not null)
                return true;

            dbRoom.UserRooms.Add(
                new UserRoom
                {
                    IdApplicationUser = user.Id,
                    IsGM = false,
                    InvitationAccepted = false,
                    ApplicationUser = user,
                });
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> AccepteInvite(Guid idRoom, string userName)
    {
        try
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user is null) return false;

            var dbUserRoom = await _context.UserRooms.FirstOrDefaultAsync(ur => ur.IdRoom == idRoom && ur.IdApplicationUser == user.Id);
            if (dbUserRoom is null) return false;

            dbUserRoom.InvitationAccepted = true;
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
    public async Task<bool> DeclineInvite(Guid idRoom, string userName)
    {
        try
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user is null) return false;

            var dbUserRoom = await _context.UserRooms.FirstOrDefaultAsync(ur => ur.IdRoom == idRoom && ur.IdApplicationUser == user.Id);
            if (dbUserRoom is null) return false;

            _context.UserRooms.Remove(dbUserRoom);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

}
