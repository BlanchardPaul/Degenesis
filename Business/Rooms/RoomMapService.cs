using Business.Hubs;
using DataAccessLayer;
using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Rooms;
using Domain.Rooms;
using Domain.Users;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SkiaSharp;
using System.Text.Json;

namespace Business.Rooms;

public interface IRoomMapService
{
    Task<Result<List<RoomMapDto>>> GetByRoomAsync(Guid roomId, string userName);
    Task<Result<RoomMapDto>> UploadAsync(Guid roomId, IFormFile file, string userName);
    Task<Result<List<RoomMapTokenDto>>> GetTokensAsync(Guid mapId, string userName);
    Task<Result<List<RoomMapDrawingDto>>> GetDrawingsAsync(Guid mapId, string userName);
    Task<Result<object>> SaveDrawingAsync(Guid mapId, RoomMapDrawingDto drawing, string userName);
    Task<Result<object>> DeleteDrawingAsync(Guid mapId, Guid drawingId, string userName);
    Task<Result<object>> SaveTokensAsync(Guid mapId, List<RoomMapTokenDto> tokens, string userName);
    Task<Result<object>> SaveMapStateAsync(Guid mapId, RoomMapStateDto state, string userName);
    Task<Result<object>> SaveGridConfigurationAsync(Guid mapId, int gridCellSize, bool isGridVisible, string userName);
    Task<Result<object>> SelectMapAsync(Guid roomId, Guid mapId, string userName);
    Task<Result<object>> DeleteAsync(Guid mapId, string userName);
}

public class RoomMapService : IRoomMapService
{
    private const long MaxMapSizeBytes = 20 * 1024 * 1024;
    private const long MaxImagePixels = 80_000_000;
    private static readonly string[] AllowedExtensions = [".jpg", ".jpeg", ".png", ".webp"];

    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IRoomMapBroadcastService _broadcast;
    private readonly string _mapsFolder;

    public RoomMapService(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment env, IRoomMapBroadcastService broadcast)
    {
        _context = context;
        _userManager = userManager;
        _broadcast = broadcast;
        var webRootPath = env.WebRootPath ?? Path.Combine(env.ContentRootPath, "wwwroot");
        _mapsFolder = Path.Combine(webRootPath, "map-images");
    }

    public async Task<Result<List<RoomMapDto>>> GetByRoomAsync(Guid roomId, string userName)
    {
        try
        {
            var accessError = await ValidateRoomMemberAsync(roomId, userName);
            if (accessError is not null)
                return new Result<List<RoomMapDto>> { IsError = true, Error = accessError };

            var maps = await _context.RoomMaps
                .Where(map => map.IdRoom == roomId)
                .Select(map => new RoomMapDto
                {
                    Id = map.Id,
                    IdRoom = map.IdRoom,
                    Name = map.Name,
                    FileName = map.FileName,
                    Width = map.Width,
                    Height = map.Height,
                    GridCellSize = map.GridCellSize,
                    IsGridVisible = map.IsGridVisible
                })
                .ToListAsync();

            return new Result<List<RoomMapDto>> { Value = maps };
        }
        catch
        {
            return new Result<List<RoomMapDto>> { IsError = true, Error = "An error occurred while retrieving maps" };
        }
    }

    public async Task<Result<RoomMapDto>> UploadAsync(Guid roomId, IFormFile file, string userName)
    {
        try
        {
            var accessError = await ValidateGmAccessAsync(roomId, userName);
            if (accessError is not null)
                return new Result<RoomMapDto> { IsError = true, Error = accessError };

            if (file is null || file.Length == 0)
                return new Result<RoomMapDto> { IsError = true, Error = "No image file provided" };
            if (file.Length > MaxMapSizeBytes)
                return new Result<RoomMapDto> { IsError = true, Error = "Image too large (max 20 MB)" };

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!AllowedExtensions.Contains(extension))
                return new Result<RoomMapDto> { IsError = true, Error = "Only JPG, PNG or WEBP images are allowed" };

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            var imageBytes = memoryStream.ToArray();
            if (!TryGetImageDimensions(imageBytes, out var width, out var height))
                return new Result<RoomMapDto> { IsError = true, Error = "Invalid image file" };
            if ((long)width * height > MaxImagePixels)
                return new Result<RoomMapDto> { IsError = true, Error = "Image dimensions are too large" };

            Directory.CreateDirectory(_mapsFolder);
            var mapId = Guid.NewGuid();
            var fileName = $"{mapId:N}{extension}";
            var imagePath = Path.Combine(_mapsFolder, fileName);
            var map = new RoomMap
            {
                Id = mapId,
                IdRoom = roomId,
                Name = Path.GetFileNameWithoutExtension(file.FileName).Trim() is { Length: > 0 } name
                    ? name[..Math.Min(name.Length, 200)]
                    : "Map",
                FileName = fileName,
                Width = width,
                Height = height,
                GridCellSize = Math.Min(300, Math.Max(1, (int)Math.Ceiling(Math.Max(width, height) / 10d))),
                IsGridVisible = true
            };

            await File.WriteAllBytesAsync(imagePath, imageBytes);
            try
            {
                _context.RoomMaps.Add(map);
                await _context.SaveChangesAsync();
            }
            catch
            {
                File.Delete(imagePath);
                throw;
            }

            return new Result<RoomMapDto>
            {
                Value = new RoomMapDto
                {
                    Id = map.Id,
                    IdRoom = map.IdRoom,
                    Name = map.Name,
                    FileName = map.FileName,
                    Width = map.Width,
                    Height = map.Height,
                    GridCellSize = map.GridCellSize,
                    IsGridVisible = map.IsGridVisible
                }
            };
        }
        catch
        {
            return new Result<RoomMapDto> { IsError = true, Error = "An error occurred while uploading the map" };
        }
    }

    public async Task<Result<List<RoomMapTokenDto>>> GetTokensAsync(Guid mapId, string userName)
    {
        try
        {
            var map = await _context.RoomMaps.FirstOrDefaultAsync(item => item.Id == mapId);
            if (map is null)
                return new Result<List<RoomMapTokenDto>> { IsError = true, Error = "Map not found" };

            var accessError = await ValidateRoomMemberAsync(map.IdRoom, userName);
            if (accessError is not null)
                return new Result<List<RoomMapTokenDto>> { IsError = true, Error = accessError };

            var savedTokens = await _context.RoomMapTokens
                .Where(token => token.IdRoomMap == mapId)
                .ToListAsync();

            var tokens = new List<RoomMapTokenDto>();
            foreach (var savedToken in savedTokens)
            {
                if (savedToken.IsNpc)
                {
                    var npc = await _context.NPCs.AsNoTracking()
                        .FirstOrDefaultAsync(item => item.Id == savedToken.MemberId);
                    if (npc is not null)
                    {
                        tokens.Add(new RoomMapTokenDto
                        {
                            MemberId = savedToken.MemberId,
                            IsNpc = true,
                            Name = npc.Name,
                            AvatarName = npc.AvatarName,
                            X = savedToken.X,
                            Y = savedToken.Y
                        });
                    }
                }
                else
                {
                    var character = await _context.Characters.AsNoTracking()
                        .FirstOrDefaultAsync(item => item.Id == savedToken.MemberId);
                    if (character is not null)
                    {
                        tokens.Add(new RoomMapTokenDto
                        {
                            MemberId = savedToken.MemberId,
                            Name = character.Name,
                            AvatarName = character.AvatarName,
                            X = savedToken.X,
                            Y = savedToken.Y
                        });
                    }
                }
            }

            return new Result<List<RoomMapTokenDto>> { Value = tokens };
        }
        catch
        {
            return new Result<List<RoomMapTokenDto>> { IsError = true, Error = "An error occurred while retrieving tokens" };
        }
    }

    public async Task<Result<object>> SelectMapAsync(Guid roomId, Guid mapId, string userName)
    {
        try
        {
            var accessError = await ValidateGmAccessAsync(roomId, userName);
            if (accessError is not null)
                return new Result<object> { IsError = true, Error = accessError };

            var room = await _context.Rooms.FirstOrDefaultAsync(item => item.Id == roomId);
            if (room is null)
                return new Result<object> { IsError = true, Error = "Room not found" };

            var mapExists = await _context.RoomMaps
                .AnyAsync(item => item.Id == mapId && item.IdRoom == roomId);
            if (!mapExists)
                return new Result<object> { IsError = true, Error = "Map not found" };

            room.SelectedMapId = mapId;
            await _context.SaveChangesAsync();
            await _broadcast.NotifyMapSelectedAsync(roomId, mapId);
            return new Result<object> { Value = null };
        }
        catch
        {
            return new Result<object> { IsError = true, Error = "An error occurred while selecting the map" };
        }
    }

    public async Task<Result<List<RoomMapDrawingDto>>> GetDrawingsAsync(Guid mapId, string userName)
    {
        try
        {
            var map = await _context.RoomMaps.FirstOrDefaultAsync(item => item.Id == mapId);
            if (map is null)
                return new Result<List<RoomMapDrawingDto>> { IsError = true, Error = "Map not found" };

            var accessError = await ValidateRoomMemberAsync(map.IdRoom, userName);
            if (accessError is not null)
                return new Result<List<RoomMapDrawingDto>> { IsError = true, Error = accessError };

            var drawings = JsonSerializer.Deserialize<List<RoomMapDrawingDto>>(map.DrawingsJson);
            return new Result<List<RoomMapDrawingDto>> { Value = drawings ?? [] };
        }
        catch
        {
            return new Result<List<RoomMapDrawingDto>> { IsError = true, Error = "An error occurred while retrieving map drawings" };
        }
    }

    public async Task<Result<object>> SaveDrawingAsync(Guid mapId, RoomMapDrawingDto drawing, string userName)
    {
        try
        {
            await using var transaction = await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);
            var map = await _context.RoomMaps.FirstOrDefaultAsync(item => item.Id == mapId);
            if (map is null)
                return new Result<object> { IsError = true, Error = "Map not found" };

            var accessError = await ValidateRoomMemberAsync(map.IdRoom, userName);
            if (accessError is not null)
                return new Result<object> { IsError = true, Error = accessError };

            if (!IsValidDrawing(drawing, map.Width, map.Height))
                return new Result<object> { IsError = true, Error = "Map drawing contains invalid data" };

            var drawings = JsonSerializer.Deserialize<List<RoomMapDrawingDto>>(map.DrawingsJson) ?? [];
            if (drawings.Count >= 1000)
                return new Result<object> { IsError = true, Error = "The map has reached the maximum number of drawings" };
            if (drawings.Any(existing => existing.Id == drawing.Id))
                return new Result<object> { Value = null };

            drawings.Add(drawing);
            map.DrawingsJson = JsonSerializer.Serialize(drawings);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            await _broadcast.NotifyDrawingAddedAsync(map.IdRoom, map.Id, drawing);
            return new Result<object> { Value = null };
        }
        catch
        {
            return new Result<object> { IsError = true, Error = "An error occurred while saving the map drawing" };
        }
    }

    public async Task<Result<object>> DeleteDrawingAsync(Guid mapId, Guid drawingId, string userName)
    {
        try
        {
            await using var transaction = await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);
            var map = await _context.RoomMaps.FirstOrDefaultAsync(item => item.Id == mapId);
            if (map is null)
                return new Result<object> { IsError = true, Error = "Map not found" };

            var accessError = await ValidateGmAccessAsync(map.IdRoom, userName);
            if (accessError is not null)
                return new Result<object> { IsError = true, Error = accessError };

            var drawings = JsonSerializer.Deserialize<List<RoomMapDrawingDto>>(map.DrawingsJson) ?? [];
            if (drawings.RemoveAll(drawing => drawing.Id == drawingId) == 0)
                return new Result<object> { IsError = true, Error = "Map drawing not found" };

            map.DrawingsJson = JsonSerializer.Serialize(drawings);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            await _broadcast.NotifyDrawingDeletedAsync(map.IdRoom, map.Id, drawingId);
            return new Result<object> { Value = null };
        }
        catch
        {
            return new Result<object> { IsError = true, Error = "An error occurred while deleting the map drawing" };
        }
    }

    public async Task<Result<object>> SaveTokensAsync(Guid mapId, List<RoomMapTokenDto> tokens, string userName)
    {
        try
        {
            var map = await _context.RoomMaps.FirstOrDefaultAsync(item => item.Id == mapId);
            if (map is null)
                return new Result<object> { IsError = true, Error = "Map not found" };

            var user = await _userManager.FindByNameAsync(userName);
            if (user is null)
                return new Result<object> { IsError = true, Error = "User not found" };

            var roomMembership = await _context.UserRooms.FirstOrDefaultAsync(userRoom =>
                userRoom.IdRoom == map.IdRoom && userRoom.IdApplicationUser == user.Id);
            if (roomMembership is null || (!roomMembership.IsGM && !roomMembership.InvitationAccepted))
                return new Result<object> { IsError = true, Error = "Room access denied" };

            var existing = await _context.RoomMapTokens
                .Where(token => token.IdRoomMap == mapId)
                .ToListAsync();

            if (roomMembership.IsGM)
            {
                _context.RoomMapTokens.RemoveRange(existing);

                foreach (var token in tokens)
                {
                    _context.RoomMapTokens.Add(new RoomMapToken
                    {
                        Id = Guid.NewGuid(),
                        IdRoomMap = mapId,
                        MemberId = token.MemberId,
                        IsNpc = token.IsNpc,
                        X = token.X,
                        Y = token.Y
                    });
                }
            }
            else
            {
                var tokenKeys = tokens.Select(token => (token.MemberId, token.IsNpc)).ToList();
                if (tokenKeys.Distinct().Count() != tokens.Count || tokens.Count != existing.Count ||
                    tokens.Any(token => !double.IsFinite(token.X) || !double.IsFinite(token.Y) ||
                                        token.X < 0 || token.X > map.Width || token.Y < 0 || token.Y > map.Height))
                    return new Result<object> { IsError = true, Error = "Players can only move existing map tokens" };

                foreach (var token in tokens)
                {
                    var savedToken = existing.FirstOrDefault(item => item.MemberId == token.MemberId && item.IsNpc == token.IsNpc);
                    if (savedToken is null)
                        return new Result<object> { IsError = true, Error = "Players can only move existing map tokens" };

                    savedToken.X = token.X;
                    savedToken.Y = token.Y;
                }
            }

            await _context.SaveChangesAsync();
            var savedTokens = await GetTokensAsync(mapId, userName);
            if (savedTokens.IsError || savedTokens.Value is null)
                return new Result<object> { IsError = true, Error = savedTokens.Error ?? "Unable to load saved map tokens" };

            var drawings = JsonSerializer.Deserialize<List<RoomMapDrawingDto>>(map.DrawingsJson) ?? [];
            await _broadcast.NotifyMapStateUpdatedAsync(map.IdRoom, map.Id, new RoomMapStateDto
            {
                Tokens = savedTokens.Value,
                Drawings = drawings
            });
            return new Result<object> { Value = null };
        }
        catch
        {
            return new Result<object> { IsError = true, Error = "An error occurred while saving tokens" };
        }
    }

    public async Task<Result<object>> SaveMapStateAsync(Guid mapId, RoomMapStateDto state, string userName)
    {
        try
        {
            var map = await _context.RoomMaps.FirstOrDefaultAsync(item => item.Id == mapId);
            if (map is null)
                return new Result<object> { IsError = true, Error = "Map not found" };

            var accessError = await ValidateGmAccessAsync(map.IdRoom, userName);
            if (accessError is not null)
                return new Result<object> { IsError = true, Error = accessError };

            if (state is null || state.Tokens is null || state.Drawings is null ||
                state.Drawings.Count > 1000 || state.Drawings.Any(drawing => !IsValidDrawing(drawing, map.Width, map.Height)) ||
                state.Drawings.Select(drawing => drawing.Id).Distinct().Count() != state.Drawings.Count)
                return new Result<object> { IsError = true, Error = "Map drawings contain invalid data" };

            var existingTokens = await _context.RoomMapTokens
                .Where(token => token.IdRoomMap == mapId)
                .ToListAsync();
            _context.RoomMapTokens.RemoveRange(existingTokens);

            foreach (var token in state.Tokens)
            {
                _context.RoomMapTokens.Add(new RoomMapToken
                {
                    Id = Guid.NewGuid(),
                    IdRoomMap = mapId,
                    MemberId = token.MemberId,
                    IsNpc = token.IsNpc,
                    X = token.X,
                    Y = token.Y
                });
            }

            map.DrawingsJson = JsonSerializer.Serialize(state.Drawings);
            await _context.SaveChangesAsync();
            await _broadcast.NotifyMapStateUpdatedAsync(map.IdRoom, map.Id, state);
            return new Result<object> { Value = null };
        }
        catch
        {
            return new Result<object> { IsError = true, Error = "An error occurred while saving the map" };
        }
    }

    public async Task<Result<object>> SaveGridConfigurationAsync(Guid mapId, int gridCellSize, bool isGridVisible, string userName)
    {
        try
        {
            var map = await _context.RoomMaps.FirstOrDefaultAsync(item => item.Id == mapId);
            if (map is null)
                return new Result<object> { IsError = true, Error = "Map not found" };

            var accessError = await ValidateGmAccessAsync(map.IdRoom, userName);
            if (accessError is not null)
                return new Result<object> { IsError = true, Error = accessError };

            var maximumSize = Math.Min(300, Math.Max(map.Width, map.Height));
            var minimumSize = Math.Min(maximumSize, Math.Max(1, (int)Math.Ceiling(Math.Max(map.Width, map.Height) / 200d)));
            if (gridCellSize < minimumSize || gridCellSize > maximumSize)
                return new Result<object> { IsError = true, Error = $"Grid cell size must be between {minimumSize} and {maximumSize}" };

            map.GridCellSize = gridCellSize;
            map.IsGridVisible = isGridVisible;
            await _context.SaveChangesAsync();
            await _broadcast.NotifyGridConfigurationUpdatedAsync(map.IdRoom, map.Id, gridCellSize, isGridVisible);
            return new Result<object> { Value = null };
        }
        catch
        {
            return new Result<object> { IsError = true, Error = "An error occurred while saving the grid configuration" };
        }
    }

    public async Task<Result<object>> DeleteAsync(Guid mapId, string userName)
    {
        try
        {
            var map = await _context.RoomMaps.FirstOrDefaultAsync(item => item.Id == mapId);
            if (map is null)
                return new Result<object> { IsError = true, Error = "Map not found" };

            var accessError = await ValidateGmAccessAsync(map.IdRoom, userName);
            if (accessError is not null)
                return new Result<object> { IsError = true, Error = accessError };

            var imagePath = Path.Combine(_mapsFolder, map.FileName);
            _context.RoomMaps.Remove(map);
            await _context.SaveChangesAsync();

            if (File.Exists(imagePath))
                File.Delete(imagePath);

            return new Result<object> { Value = null };
        }
        catch
        {
            return new Result<object> { IsError = true, Error = "An error occurred while deleting the map" };
        }
    }

    private static bool IsValidDrawing(RoomMapDrawingDto? drawing, int mapWidth, int mapHeight)
        => drawing is not null && drawing.Id != Guid.Empty && drawing.StrokeWidth is >= 1 and <= 20 &&
           !string.IsNullOrEmpty(drawing.Color) && drawing.Color[0] == '#' && drawing.Color.Length is 7 or 9 &&
           drawing.Color.AsSpan(1).ToString().All(Uri.IsHexDigit) &&
           drawing.Points is { Count: >= 1 and <= 10000 } &&
           drawing.Points.All(point => point is not null && double.IsFinite(point.X) && double.IsFinite(point.Y) &&
                                       point.X >= 0 && point.X <= mapWidth && point.Y >= 0 && point.Y <= mapHeight);

    private async Task<string?> ValidateRoomMemberAsync(Guid roomId, string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);
        if (user is null)
            return "User not found";

        var isMember = await _context.UserRooms.AnyAsync(userRoom =>
            userRoom.IdRoom == roomId && userRoom.IdApplicationUser == user.Id &&
            (userRoom.IsGM || userRoom.InvitationAccepted));
        return isMember ? null : "Room access denied";
    }

    private async Task<string?> ValidateGmAccessAsync(Guid roomId, string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);
        if (user is null)
            return "User not found";

        var isGm = await _context.UserRooms.AnyAsync(userRoom =>
            userRoom.IdRoom == roomId && userRoom.IdApplicationUser == user.Id && userRoom.IsGM);
        return isGm ? null : "Only the room GM can manage maps";
    }

    private static bool TryGetImageDimensions(byte[] imageBytes, out int width, out int height)
    {
        width = 0;
        height = 0;
        try
        {
            using var data = SKData.CreateCopy(imageBytes);
            using var codec = SKCodec.Create(data);
            if (codec is null || codec.Info.Width <= 0 || codec.Info.Height <= 0)
                return false;

            width = codec.Info.Width;
            height = codec.Info.Height;
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
