using AutoMapper;
using Business.Hubs;
using DataAccessLayer;
using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.NPCs;
using Domain.NPCs;
using Domain.Users;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SkiaSharp;

namespace Business.NPCs;
public interface INPCService
{
    Task<Result<List<NPCDto>>> GetAllByIdRoomAsync(Guid roomId);
    Task<Result<NPCDto>> GetByIdAsync(Guid npcId);
    Task<Result<object>> CreateAsync(Guid baseNPCId, Guid roomId);
    Task<Result<object>> UpdateAsync(NPCDto npcDto);
    Task<Result<object>> SetAvatarAsync(Guid npcId, IFormFile file, string userName);
    Task<Result<object>> RemoveAvatarAsync(Guid npcId, string userName);
    Task<Result<object>> DeleteAsync(Guid npcId);
}

public class NPCService(ApplicationDbContext context, IMapper mapper, IRoomBroadcastService broadcastService,
    UserManager<ApplicationUser> userManager, IWebHostEnvironment env) : INPCService
{
    private readonly ApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;
    private readonly IRoomBroadcastService _broadcast = broadcastService;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly string _npcAvatarsFolder = Path.Combine(env.WebRootPath!, "npc-avatars");

    private const int RequiredAvatarWidth = 512;
    private const int RequiredAvatarHeight = 512;
    private const long MaxAvatarSizeBytes = 2 * 1024 * 1024;
    private static readonly string[] AllowedAvatarExtensions = [".jpg", ".jpeg", ".png", ".webp"];

    public async Task<Result<List<NPCDto>>> GetAllByIdRoomAsync(Guid roomId)
    {
        try
        {
            var npcs = await _context.NPCs
                .Include(n => n.NPCAttributes)
                    .ThenInclude(na => na.Attribute)
                .Include(n => n.NPCSkills)
                    .ThenInclude(ns => ns.Skill)
                .OrderBy(n => n.Name)
                .Where(n => n.IdRoom == roomId)
                .ToListAsync();
            return new Result<List<NPCDto>> { Value = _mapper.Map<List<NPCDto>>(npcs) };
        }
        catch
        {
            return new Result<List<NPCDto>>() { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<NPCDto>> GetByIdAsync(Guid npcId)
    {
        try
        {
            var npc = await _context.NPCs
                .Include(n => n.NPCAttributes)
                    .ThenInclude(na => na.Attribute)
                .Include(n => n.NPCSkills)
                    .ThenInclude(ns => ns.Skill)
                .FirstOrDefaultAsync(n => n.Id == npcId);

            if(npc is null)
                return new Result<NPCDto>() { IsError = true, Error = "NPC not found" };

            return new Result<NPCDto> { Value = _mapper.Map<NPCDto>(npc) };
        }
        catch
        {
            return new Result<NPCDto>() { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> CreateAsync(Guid baseNPCId, Guid roomId)
    {
        try
        {
            var baseNpc = await _context.BaseNPCs
                .Include(bn => bn.BaseNPCAttributes)
                .Include(bn => bn.BaseNPCSkills)
                .AsNoTracking()
                .FirstOrDefaultAsync(bn => bn.Id == baseNPCId);
            if (baseNpc is null)
                return new Result<object> { IsError = true, Error = "BaseNPC not found" };

            var room = await _context.Rooms.FindAsync(roomId);
            if (room is null)
                return new Result<object> { IsError = true, Error = "Room not found" };

            var npc = _mapper.Map<NPC>(baseNpc);
            npc.Id = Guid.NewGuid();
            npc.IdRoom = roomId;
            npc.Room = room;

            foreach (var baseNpcAttribute in baseNpc.BaseNPCAttributes)
            {
                npc.NPCAttributes.Add(new NPCAttribute
                {
                    AttributeId = baseNpcAttribute.AttributeId,
                    NPCId = npc.Id,
                    NPC = npc,
                    Level = baseNpcAttribute.Level
                });
            }

            foreach (var baseNpcSkill in baseNpc.BaseNPCSkills)
            {
                npc.NPCSkills.Add(new NPCSkill
                {
                    SkillId = baseNpcSkill.SkillId,
                    NPCId = npc.Id,
                    NPC = npc,
                    Level = baseNpcSkill.Level
                });
            }

            _context.NPCs.Add(npc);
            await _context.SaveChangesAsync();
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> UpdateAsync(NPCDto npcDto)
    {
        try
        {
            var existingNpc = await _context.NPCs
                .Include(n => n.NPCAttributes)
                .Include(n => n.NPCSkills)
                .FirstOrDefaultAsync(n => n.Id == npcDto.Id);
            if (existingNpc is null)
                return new Result<object> { IsError = true, Error = "NPC not found" };

            _mapper.Map(npcDto, existingNpc);

            foreach (var attribute in npcDto.Attributes)
            {
                var link = existingNpc.NPCAttributes
                    .FirstOrDefault(na => na.AttributeId == attribute.AttributeId);
                if (link is not null)
                    link.Level = attribute.Level;
            }

            foreach (var skill in npcDto.Skills)
            {
                var link = existingNpc.NPCSkills
                    .FirstOrDefault(ns => ns.SkillId == skill.SkillId);
                if (link is not null)
                    link.Level = skill.Level;
            }

            await _context.SaveChangesAsync();
            await _broadcast.NotifyNpcUpdatedAsync(npcDto.Id);
            return new Result<object> { Value = null };
        }
        catch
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> SetAvatarAsync(Guid npcId, IFormFile file, string userName)
    {
        try
        {
            var npc = await _context.NPCs.FirstOrDefaultAsync(n => n.Id == npcId);
            if (npc is null)
                return new Result<object> { IsError = true, Error = "NPC not found" };

            var authorizationError = await ValidateGmAccessAsync(npc.IdRoom, userName);
            if (authorizationError is not null)
                return authorizationError;

            if (file.Length == 0)
                return new Result<object> { IsError = true, Error = "No file provided" };
            if (file.Length > MaxAvatarSizeBytes)
                return new Result<object> { IsError = true, Error = "File too large (max 2 MB)" };

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!AllowedAvatarExtensions.Contains(extension))
                return new Result<object> { IsError = true, Error = "Only JPG, PNG or WEBP allowed" };

            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);

            var dimensionError = ValidateAvatarDimensions(ms);
            if (dimensionError is not null)
                return dimensionError;

            Directory.CreateDirectory(_npcAvatarsFolder);
            var fileName = $"{npcId}{extension}";
            var fullPath = Path.Combine(_npcAvatarsFolder, fileName);
            foreach (var old in Directory.GetFiles(_npcAvatarsFolder, $"{npcId}.*"))
                if (!string.Equals(old, fullPath, StringComparison.OrdinalIgnoreCase))
                    File.Delete(old);

            ms.Position = 0;
            await using (var fs = File.Create(fullPath))
                await ms.CopyToAsync(fs);

            npc.AvatarName = fileName;
            await _context.SaveChangesAsync();
            await _broadcast.NotifyNpcUpdatedAsync(npcId);
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> RemoveAvatarAsync(Guid npcId, string userName)
    {
        try
        {
            var npc = await _context.NPCs.FirstOrDefaultAsync(n => n.Id == npcId);
            if (npc is null)
                return new Result<object> { IsError = true, Error = "NPC not found" };

            var authorizationError = await ValidateGmAccessAsync(npc.IdRoom, userName);
            if (authorizationError is not null)
                return authorizationError;

            if (npc.AvatarName is not null)
            {
                var fullPath = Path.Combine(_npcAvatarsFolder, npc.AvatarName);
                if (File.Exists(fullPath))
                    File.Delete(fullPath);
            }

            npc.AvatarName = null;
            await _context.SaveChangesAsync();
            await _broadcast.NotifyNpcUpdatedAsync(npcId);
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }

    private async Task<Result<object>?> ValidateGmAccessAsync(Guid roomId, string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);
        if (user is null)
            return new Result<object> { IsError = true, Error = "User not found" };

        var isGm = await _context.UserRooms.AnyAsync(ur =>
            ur.IdApplicationUser == user.Id && ur.IdRoom == roomId && ur.IsGM);
        return isGm ? null : new Result<object> { IsError = true, Error = "Only the room GM can manage NPC avatars" };
    }

    private static Result<object>? ValidateAvatarDimensions(MemoryStream ms)
    {
        try
        {
            ms.Position = 0;
            using var data = SKData.Create(ms);
            using var codec = SKCodec.Create(data);
            if (codec is null)
                return new Result<object> { IsError = true, Error = "Invalid image file" };

            var width = codec.Info.Width;
            var height = codec.Info.Height;
            if (width != RequiredAvatarWidth || height != RequiredAvatarHeight)
                return new Result<object>
                {
                    IsError = true,
                    Error = $"Avatar must be exactly {RequiredAvatarWidth}x{RequiredAvatarHeight} px (received {width}x{height})"
                };

            return null;
        }
        catch
        {
            return new Result<object> { IsError = true, Error = "Invalid image file" };
        }
    }

    public async Task<Result<object>> DeleteAsync(Guid id)
    {
        try
        {
            var existingNPC = await _context.NPCs.FirstOrDefaultAsync(b => b.Id == id);
            if (existingNPC is null)
                return new Result<object> { IsError = true, Error = "NPC not found" };

            if (existingNPC.AvatarName is not null)
            {
                var avatarPath = Path.Combine(_npcAvatarsFolder, existingNPC.AvatarName);
                if (File.Exists(avatarPath))
                    File.Delete(avatarPath);
            }

            _context.NPCs.Remove(existingNPC);
            await _context.SaveChangesAsync();
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }
}
