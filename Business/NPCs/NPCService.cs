using DataAccessLayer;
using Domain.NPCs;
using Microsoft.EntityFrameworkCore;

namespace Business.NPCs;
public interface INPCService
{
    Task<List<NPC>> GetAllNPCsAsync();
    Task<NPC?> GetNPCByIdAsync(Guid id);
    Task<NPC> CreateNPCAsync(NPC npc);
    Task<NPC?> UpdateNPCAsync(Guid id, NPC npc);
    Task<bool> DeleteNPCAsync(Guid id);
}

public class NPCService : INPCService
{
    private readonly ApplicationDbContext _context;

    public NPCService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<NPC>> GetAllNPCsAsync()
    {
        return await _context.NPCs.ToListAsync();
    }

    public async Task<NPC?> GetNPCByIdAsync(Guid id)
    {
        return await _context.NPCs.FindAsync(id);
    }

    public async Task<NPC> CreateNPCAsync(NPC npc)
    {
        _context.NPCs.Add(npc);
        await _context.SaveChangesAsync();
        return npc;
    }

    public async Task<NPC?> UpdateNPCAsync(Guid id, NPC npc)
    {
        var existingNPC = await _context.NPCs.FindAsync(id);
        if (existingNPC is null) return null;

        _context.Entry(existingNPC).CurrentValues.SetValues(npc);
        await _context.SaveChangesAsync();
        return existingNPC;
    }

    public async Task<bool> DeleteNPCAsync(Guid id)
    {
        var npc = await _context.NPCs.FindAsync(id);
        if (npc is null) return false;

        _context.NPCs.Remove(npc);
        await _context.SaveChangesAsync();
        return true;
    }
}
