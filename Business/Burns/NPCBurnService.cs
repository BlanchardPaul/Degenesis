using DataAccessLayer;
using Domain.Burns;
using Microsoft.EntityFrameworkCore;

namespace Business.Burns;
public interface INPCBurnService
{
    Task<List<NPCBurn>> GetAllNPCBurnsAsync();
    Task<NPCBurn?> GetNPCBurnByIdAsync(Guid npcId, Guid burnId);
    Task<NPCBurn> CreateNPCBurnAsync(NPCBurn npcBurn);
    Task<NPCBurn?> UpdateNPCBurnAsync(Guid npcId, Guid burnId, NPCBurn npcBurn);
    Task<bool> DeleteNPCBurnAsync(Guid npcId, Guid burnId);
}
public class NPCBurnService : INPCBurnService
{
    private readonly ApplicationDbContext _context;

    public NPCBurnService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<NPCBurn>> GetAllNPCBurnsAsync()
    {
        return await _context.NPCBurns.ToListAsync();
    }

    public async Task<NPCBurn?> GetNPCBurnByIdAsync(Guid npcId, Guid burnId)
    {
        return await _context.NPCBurns.FindAsync(npcId, burnId);
    }

    public async Task<NPCBurn> CreateNPCBurnAsync(NPCBurn npcBurn)
    {
        _context.NPCBurns.Add(npcBurn);
        await _context.SaveChangesAsync();
        return npcBurn;
    }

    public async Task<NPCBurn?> UpdateNPCBurnAsync(Guid npcId, Guid burnId, NPCBurn npcBurn)
    {
        var existingNPCBurn = await _context.NPCBurns.FindAsync(npcId, burnId);

        if (existingNPCBurn is null)
        {
            return null;
        }

        existingNPCBurn = npcBurn;

        await _context.SaveChangesAsync();
        return existingNPCBurn;
    }

    public async Task<bool> DeleteNPCBurnAsync(Guid npcId, Guid burnId)
    {
        var existingNPCBurn = await _context.NPCBurns.FindAsync(npcId, burnId);

        if (existingNPCBurn is null)
        {
            return false;
        }

        _context.NPCBurns.Remove(existingNPCBurn);
        await _context.SaveChangesAsync();
        return true;
    }
}
