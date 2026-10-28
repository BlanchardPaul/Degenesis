using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs;
using Domain.NPCs;
using Microsoft.EntityFrameworkCore;

namespace Business.NPCs;
public interface IBaseNPCService
{
    Task<Result<List<BaseNPCDto>>> GetAllAsync();
    Task<Result<BaseNPCDto>> GetByIdAsync(Guid id);
    Task<Result<object>> CreateAsync(BaseNPCCreateDto npcDto);
    Task<Result<object>> BasicInfosUpdateAsync(BaseNPCDto npcDto);
    Task<Result<object>> StatInfosUpdateAsync(BaseNPCDto npcDto);
    Task<Result<object>> DeleteAsync(Guid id);
}

public class BaseNPCService(ApplicationDbContext context, IMapper mapper) : IBaseNPCService
{
    private readonly ApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<List<BaseNPCDto>>> GetAllAsync()
    {
        try
        {
            var npcs = await _context.BaseNPCs
                .Include(n => n.NPCAttributes)
                    .ThenInclude(na => na.Attribute)
                .Include(n => n.NPCSkills)
                    .ThenInclude(ns => ns.Skill)
                .OrderBy(n => n.Name)
                .ToListAsync();
            return new Result<List<BaseNPCDto>> { Value = _mapper.Map<List<BaseNPCDto>>(npcs) };
        }
        catch (Exception)
        {
            return new Result<List<BaseNPCDto>> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<BaseNPCDto>> GetByIdAsync(Guid id)
    {
        try
        {
            var npc = await _context.BaseNPCs
                .Include(n => n.NPCAttributes)
                    .ThenInclude(na => na.Attribute)
                .Include(n => n.NPCSkills)
                    .ThenInclude(ns => ns.Skill)
                .FirstOrDefaultAsync(n => n.Id == id);
            if (npc is null)
                return new Result<BaseNPCDto> { IsError = true, Error = "NPC not found" };

            return new Result<BaseNPCDto> { Value = _mapper.Map<BaseNPCDto>(npc) };
        }
        catch (Exception)
        {
            return new Result<BaseNPCDto> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> CreateAsync(BaseNPCCreateDto npcDto)
    {
        try
        {
            var npc = _mapper.Map<BaseNPC>(npcDto);
            npc.Id = Guid.NewGuid();

            // Link ALL existing attributes and skills at level 0
            var allAttributes = await _context.Attributes.ToListAsync();
            var allSkills = await _context.Skills.ToListAsync();

            foreach (var attribute in allAttributes)
            {
                npc.NPCAttributes.Add(new BaseNPCAttribute
                {
                    NPCId = npc.Id,
                    AttributeId = attribute.Id,
                    Attribute = attribute,
                    Level = 0
                });
            }

            foreach (var skill in allSkills)
            {
                npc.NPCSkills.Add(new BaseNPCSkill
                {
                    NPCId = npc.Id,
                    SkillId = skill.Id,
                    Skill = skill,
                    Level = 0
                });
            }

            _context.BaseNPCs.Add(npc);
            await _context.SaveChangesAsync();
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> BasicInfosUpdateAsync(BaseNPCDto npcDto)
    {
        try
        {
            var existingNPC = await _context.BaseNPCs
                .Include(n => n.NPCAttributes)
                .Include(n => n.NPCSkills)
                .FirstOrDefaultAsync(n => n.Id == npcDto.Id);
            if (existingNPC is null)
                return new Result<object> { IsError = true, Error = "NPC not found" };

            _mapper.Map(npcDto, existingNPC);

            await _context.SaveChangesAsync();
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> StatInfosUpdateAsync(BaseNPCDto npcDto)
    {
        try
        {
            var existingNPC = await _context.BaseNPCs
                .Include(n => n.NPCAttributes)
                .Include(n => n.NPCSkills)
                .FirstOrDefaultAsync(n => n.Id == npcDto.Id);
            if (existingNPC is null)
                return new Result<object> { IsError = true, Error = "NPC not found" };

            // Update attribute levels
            foreach (var attributeDto in npcDto.Attributes)
            {
                var link = existingNPC.NPCAttributes
                    .FirstOrDefault(na => na.AttributeId == attributeDto.AttributeId);
                // Unknown link: ignore
                if (link is null)
                    continue;

                link.Level = attributeDto.Level;
            }

            // Update skill levels
            foreach (var skillDto in npcDto.Skills)
            {
                var link = existingNPC.NPCSkills
                    .FirstOrDefault(ns => ns.SkillId == skillDto.SkillId);
                if (link is null)
                    continue;

                link.Level = skillDto.Level;
            }

            await _context.SaveChangesAsync();
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> DeleteAsync(Guid id)
    {
        try
        {
            var npc = await _context.BaseNPCs
                .FirstOrDefaultAsync(n => n.Id == id);
            if (npc is null)
                return new Result<object> { IsError = true, Error = "NPC not found" };

            _context.BaseNPCs.Remove(npc);
            await _context.SaveChangesAsync();
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }
}