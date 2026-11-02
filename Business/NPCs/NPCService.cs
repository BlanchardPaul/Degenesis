using AutoMapper;
using Business.Hubs;
using DataAccessLayer;
using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.NPCs;
using Domain.NPCs;
using Microsoft.EntityFrameworkCore;

namespace Business.NPCs;
public interface INPCService
{
    Task<Result<List<NPCDto>>> GetAllByIdRoomAsync(Guid roomId);
    Task<Result<NPCDto>> GetByIdAsync(Guid npcId);
    Task<Result<object>> CreateAsync(Guid baseNPCId, Guid roomId);
    Task<Result<object>> UpdateAsync(NPCDto npcDto);
    Task<Result<object>> DeleteAsync(Guid npcId);
}

public class NPCService(ApplicationDbContext context, IMapper mapper, IRoomBroadcastService broadcastService) : INPCService
{
    private readonly ApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;
    private readonly IRoomBroadcastService _broadcast = broadcastService;

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

    public async Task<Result<object>> DeleteAsync(Guid id)
    {
        try
        {
            var existingNPC = await _context.NPCs.FirstOrDefaultAsync(b => b.Id == id);
            if (existingNPC is null)
                return new Result<object> { IsError = true, Error = "NPC not found" };

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
