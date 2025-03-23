using DataAccessLayer;
using Domain._Artifacts;
using Microsoft.EntityFrameworkCore;


namespace Business._Artifacts;
public interface INPCArtifactService
{
    Task<NPCArtifact?> GetByIdAsync(Guid id);
    Task<IEnumerable<NPCArtifact>> GetByNPCIdAsync(Guid npcId);
    Task<NPCArtifact> CreateAsync(NPCArtifact npcArtifact);
    Task<bool> UpdateAsync(Guid id, NPCArtifact npcArtifact);
    Task<bool> DeleteAsync(Guid id);
}

public class NPCArtifactService : INPCArtifactService
{
    private readonly ApplicationDbContext _context;

    public NPCArtifactService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<NPCArtifact?> GetByIdAsync(Guid id)
    {
        return await _context.NPCArtifacts
            .Include(na => na.Artifact)
            .FirstOrDefaultAsync(na => na.Id == id);
    }

    public async Task<IEnumerable<NPCArtifact>> GetByNPCIdAsync(Guid npcId)
    {
        return await _context.NPCArtifacts
            .Where(na => na.NPCId == npcId)
            .Include(na => na.Artifact)
            .ToListAsync();
    }

    public async Task<NPCArtifact> CreateAsync(NPCArtifact npcArtifact)
    {
        _context.NPCArtifacts.Add(npcArtifact);
        await _context.SaveChangesAsync();
        return npcArtifact;
    }

    public async Task<bool> UpdateAsync(Guid id, NPCArtifact npcArtifact)
    {
        var existing = await _context.NPCArtifacts.FindAsync(id);
        if (existing is null) return false;

        existing.NPCId = npcArtifact.NPCId;
        existing.ArtifactId = npcArtifact.ArtifactId;
        existing.ChargeInMagazine = npcArtifact.ChargeInMagazine;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var existing = await _context.NPCArtifacts.FindAsync(id);
        if (existing is null) return false;

        _context.NPCArtifacts.Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }
}