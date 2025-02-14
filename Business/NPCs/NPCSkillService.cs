using DataAccessLayer;
using Domain.NPCs;
using Microsoft.EntityFrameworkCore;

namespace Business.NPCs;
public interface INPCSkillService
{
    Task<List<NPCSkill>> GetAllNPCSkillsAsync();
    Task<NPCSkill?> GetNPCSkillByIdAsync(Guid npcId, Guid skillId);
    Task<NPCSkill> CreateNPCSkillAsync(NPCSkill npcSkill);
    Task<NPCSkill?> UpdateNPCSkillAsync(Guid npcId, Guid skillId, NPCSkill npcSkill);
    Task<bool> DeleteNPCSkillAsync(Guid npcId, Guid skillId);
}

public class NPCSkillService : INPCSkillService
{
    private readonly ApplicationDbContext _context;

    public NPCSkillService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<NPCSkill>> GetAllNPCSkillsAsync()
    {
        return await _context.NPCSkills.ToListAsync();
    }

    public async Task<NPCSkill?> GetNPCSkillByIdAsync(Guid npcId, Guid skillId)
    {
        return await _context.NPCSkills
            .FirstOrDefaultAsync(ns => ns.NPCId == npcId && ns.SkillId == skillId);
    }

    public async Task<NPCSkill> CreateNPCSkillAsync(NPCSkill npcSkill)
    {
        _context.NPCSkills.Add(npcSkill);
        await _context.SaveChangesAsync();
        return npcSkill;
    }

    public async Task<NPCSkill?> UpdateNPCSkillAsync(Guid npcId, Guid skillId, NPCSkill npcSkill)
    {
        var existingNPCSkill = await _context.NPCSkills
            .FirstOrDefaultAsync(ns => ns.NPCId == npcId && ns.SkillId == skillId);

        if (existingNPCSkill == null) return null;

        _context.Entry(existingNPCSkill).CurrentValues.SetValues(npcSkill);
        await _context.SaveChangesAsync();
        return existingNPCSkill;
    }

    public async Task<bool> DeleteNPCSkillAsync(Guid npcId, Guid skillId)
    {
        var npcSkill = await _context.NPCSkills
            .FirstOrDefaultAsync(ns => ns.NPCId == npcId && ns.SkillId == skillId);

        if (npcSkill == null) return false;

        _context.NPCSkills.Remove(npcSkill);
        await _context.SaveChangesAsync();
        return true;
    }
}
