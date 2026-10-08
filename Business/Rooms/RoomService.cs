using AutoMapper;
using Business.Characters;
using DataAccessLayer;
using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Characters.CRUD;
using Degenesis.Shared.DTOs.Rooms;
using Domain.Rooms;
using Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Business.Rooms;

public interface IRoomService
{
    Task<Result<List<RoomDisplayDto>>> GetAllAsync(string userName);
    Task<Result<RoomDisplayDto>> GetByIdAsync(Guid id);
    Task<Result<object>> CreateAsync(RoomCreateDto roomCreate, string userName);
    Task<Result<object>> UpdateAsync(RoomDto room);
    Task<Result<object>> DeleteRoomAsync(Guid id);
    Task<Result<object>> InviteUser(InvitationDto invitationDto);
    Task<Result<object>> AccepteInvite(Guid idRoom, string userName);
    Task<Result<object>> DeclineInvite(Guid idRoom, string userName);
}

public class RoomService(ApplicationDbContext context, IMapper mapper, UserManager<ApplicationUser> userManager, ICharacterService characterService) : IRoomService
{
    private readonly ApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly ICharacterService _characterService = characterService;

    public async Task<Result<List<RoomDisplayDto>>> GetAllAsync(string userName)
    {
        try
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user is null)
                return new Result<List<RoomDisplayDto>> { IsError = true, Error = "User not found" };

            var rooms = await _context.Rooms
                .Include(r => r.Characters)
                .Include(r => r.UserRooms)
                .ThenInclude(ur => ur.ApplicationUser)
                .Where(r => r.UserRooms.Any(ur => ur.IdApplicationUser == user.Id))
                .ToListAsync();

            List<RoomDisplayDto> displayRooms = [];
            foreach (var room in rooms)
            {
                displayRooms.Add(new RoomDisplayDto
                {
                    Id = room.Id,
                    Name = room.Name,
                    Description = room.Description,
                    GMName = room.UserRooms.First(ur => ur.IsGM).ApplicationUser.UserName ?? string.Empty,
                    Players = [.. room.UserRooms.Where(ur => !ur.IsGM && ur.InvitationAccepted).Select(ur => ur.ApplicationUser.UserName ?? string.Empty)],
                    Pendings = [.. room.UserRooms.Where(ur => !ur.IsGM && !ur.InvitationAccepted).Select(ur => ur.ApplicationUser.UserName ?? string.Empty)],
                    IsPendingForCurrentUser = !room.UserRooms.First(ur => ur.IdApplicationUser == user.Id).InvitationAccepted,
                    IsCurrentUserGM = room.UserRooms.First(ur => ur.IdApplicationUser == user.Id).IsGM,
                    HasCharacter = room.Characters.Exists(c => c.IdApplicationUser == user.Id)
                });
            }
            return new Result<List<RoomDisplayDto>> { Value = displayRooms };
        }
        catch (Exception)
        {
            return new Result<List<RoomDisplayDto>> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<RoomDisplayDto>> GetByIdAsync(Guid id)
    {
        try
        {
            var dbRoom = await _context.Rooms
                .Include(r => r.UserRooms)
                .ThenInclude(ur => ur.ApplicationUser)
                .FirstOrDefaultAsync(r => r.Id == id);
            if (dbRoom is null)
                return new Result<RoomDisplayDto> { IsError = true, Error = "Room not found" };

            var room = new RoomDisplayDto
            {
                Id = dbRoom.Id,
                Name = dbRoom.Name,
                Description = dbRoom.Description,
                GMName = dbRoom.UserRooms.First(ur => ur.IsGM).ApplicationUser.UserName ?? string.Empty,
                Players = [.. dbRoom.UserRooms.Where(ur => !ur.IsGM && ur.InvitationAccepted).Select(ur => ur.ApplicationUser.UserName ?? string.Empty)],
                Pendings = [.. dbRoom.UserRooms.Where(ur => !ur.IsGM && !ur.InvitationAccepted).Select(ur => ur.ApplicationUser.UserName ?? string.Empty)]

            };
            return new Result<RoomDisplayDto> { Value = room };
        }
        catch(Exception)
        {
            return new Result<RoomDisplayDto> { IsError = true, Error = "A server error occurred" };
        }

    }

    public async Task<Result<object>> CreateAsync(RoomCreateDto roomCreate, string userName)
    {
        try
        {
            var creator = await _userManager.FindByNameAsync(userName);
            if (creator is null)
                    return new Result<object> { IsError = true, Error = "User not found in UserManager" };
            var dbUser = _context.Users.Find(creator.Id);
            if (dbUser is null)
                return new Result<object> { IsError = true, Error = "User not found in data base" };

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

            return new Result<object> { Value = null };
        }
        catch(Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> UpdateAsync(RoomDto room)
    {
        try
        {
            var existing = await _context.Rooms.IgnoreAutoIncludes().FirstOrDefaultAsync(r => r.Id == room.Id);
            if (existing is null)
                return new Result<object> { IsError = true, Error = "Room not found" };

            _mapper.Map(room, existing);

            await _context.SaveChangesAsync();
            return new Result<object> { Value = null };
        }
        catch(Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }

    }

    public async Task<Result<object>> DeleteRoomAsync(Guid id)
    {
        try
        {
            var room = await _context.Rooms
                .Include(r => r.Characters)
                .FirstOrDefaultAsync(r => r.Id == id);
            if (room is null)
                return new Result<object> { IsError = true, Error = "Room not found" };

            foreach (var character in room.Characters)
            {
                var characterToDelete = await _context.Characters
                   .Include(c => c.CharacterAttributes)
                   .Include(c => c.CharacterSkills)
                   .Include(c => c.CharacterBackgrounds)
                   .Include(c => c.CharacterPontentials)
                   .FirstOrDefaultAsync(c => c.Id == character.Id);

                if (characterToDelete is null)
                    return new Result<object> { IsError = true, Error = "Character not found" };

                _context.CharacterAttributes.RemoveRange(character.CharacterAttributes);

                _context.Characters.Remove(character);
            }

            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }

    }

    public async Task<Result<object>> InviteUser(InvitationDto invitationDto)
    {
        try
        {
            var dbRoom = await _context.Rooms.FindAsync(invitationDto.IdRoom);
            if (dbRoom is null)
                return new Result<object> { IsError = true, Error = "Room not found" };

            var user = await _userManager.FindByNameAsync(invitationDto.UserName);
            if (user is null)
                return new Result<object> { IsError = true, Error = "User not found in UserManager" };

            //Check if user alreasy invited
            var existingUserRoom = await _context.UserRooms.FirstOrDefaultAsync(ur => ur.IdRoom == invitationDto.IdRoom && ur.IdApplicationUser == user.Id);
            if (existingUserRoom is not null)
                return new Result<object> { IsError = true, Error = "User already invited in room" };

            dbRoom.UserRooms.Add(
                new UserRoom
                {
                    IdApplicationUser = user.Id,
                    IsGM = false,
                    InvitationAccepted = false,
                    ApplicationUser = user,
                });
            await _context.SaveChangesAsync();
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> AccepteInvite(Guid idRoom, string userName)
    {
        try
        {
            var user = await _userManager.FindByNameAsync(userName);
            if ( user is null)
                return new Result<object> { IsError = true, Error = "User not found in UserManager" };

            var dbUserRoom = await _context.UserRooms.FirstOrDefaultAsync(ur => ur.IdRoom == idRoom && ur.IdApplicationUser == user.Id);
            if (dbUserRoom is null)
                return new Result<object> { IsError = true, Error = "UserRoom not found" };

            dbUserRoom.InvitationAccepted = true;
            await _context.SaveChangesAsync();
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> DeclineInvite(Guid idRoom, string userName)
    {
        try
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user is null)
                return new Result<object> { IsError = true, Error = "User not found in UserManager" };

            var dbUserRoom = await _context.UserRooms.FirstOrDefaultAsync(ur => ur.IdRoom == idRoom && ur.IdApplicationUser == user.Id);
            if (dbUserRoom is null)
                return new Result<object> { IsError = true, Error = "UserRoom not found" };

            //If user had a character delete it
            var character = await _context.Characters.FirstOrDefaultAsync(c => c.IdRoom == idRoom && c.IdApplicationUser == user.Id);
            if (character is not null)
            {
                var characterDeletion = await _characterService.DeleteCharacterAsync(idRoom, userName);
                if (characterDeletion.IsError)
                    throw new Exception("Error during Character Deletion");
            }

            _context.UserRooms.Remove(dbUserRoom);
            await _context.SaveChangesAsync();
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }

}
