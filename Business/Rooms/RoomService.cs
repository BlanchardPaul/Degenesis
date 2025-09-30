using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs.Rooms;
using Domain.Rooms;
using Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Business.Rooms;

public interface IRoomService
{
    Task<List<RoomDisplayDto>> GetAllAsync(string userName);
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


    public async Task<List<RoomDisplayDto>> GetAllAsync(string userName)
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
                .FirstOrDefaultAsync(r => r.Id == id) 
                ?? throw new Exception("Room not found");

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
            var creator = await _userManager.FindByNameAsync(userName) 
                ?? throw new Exception("User in UserManager not found");
            var dbUser = _context.Users.Find(creator.Id) 
                ?? throw new Exception("User in DB not found");

            var room = _mapper.Map<Room>(roomCreate);

            room.UserRooms =
            [
                new UserRoom
                {
                    IdApplicationUser = creator.Id,
                    IsGM = true,
                    InvitationAccepted = true,
                    ApplicationUser = dbUser
                }
            ];

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
        var existing = await _context.Rooms.IgnoreAutoIncludes().FirstOrDefaultAsync(r => r.Id == room.Id)
            ?? throw new Exception("Room not found"); ;

        _mapper.Map(room, existing);

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteRoomAsync(Guid id)
    {
        var room = await _context.Rooms.FindAsync(id)
            ?? throw new Exception("User in UserManager not found");

        _context.Rooms.Remove(room);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> InviteUser(InvitationDto invitationDto)
    {
        try
        {
            var dbRoom = await _context.Rooms.FindAsync(invitationDto.IdRoom)
                ?? throw new Exception("Room not found");

            var user = await _userManager.FindByNameAsync(invitationDto.UserName)
                ?? throw new Exception("User not found");

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
            var user = await _userManager.FindByNameAsync(userName)
                ?? throw new Exception("User not found");

            var dbUserRoom = await _context.UserRooms.FirstOrDefaultAsync(ur => ur.IdRoom == idRoom && ur.IdApplicationUser == user.Id)
                ?? throw new Exception("UserRoom not found");

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
            var user = await _userManager.FindByNameAsync(userName)
                ?? throw new Exception("User not found");

            var dbUserRoom = await _context.UserRooms.FirstOrDefaultAsync(ur => ur.IdRoom == idRoom && ur.IdApplicationUser == user.Id)
                ?? throw new Exception("UserRoom not found");

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
