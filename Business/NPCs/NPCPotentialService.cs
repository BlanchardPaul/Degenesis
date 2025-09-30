//using DataAccessLayer;
//using Domain.NPCs;
//using Microsoft.EntityFrameworkCore;

//namespace Business.NPCs;
//public interface INPCPotentialService
//{
//    Task<List<NPCPotential>> GetAllNPCPotentialsAsync();
//    Task<NPCPotential?> GetNPCPotentialByIdAsync(Guid npcId, Guid potentialId);
//    Task<NPCPotential> CreateNPCPotentialAsync(NPCPotential npcPotential);
//    Task<NPCPotential?> UpdateNPCPotentialAsync(Guid npcId, Guid potentialId, NPCPotential npcPotential);
//    Task<bool> DeleteNPCPotentialAsync(Guid npcId, Guid potentialId);
//}

//public class NPCPotentialService : INPCPotentialService
//{
//    private readonly ApplicationDbContext _context;

//    public NPCPotentialService(ApplicationDbContext context)
//    {
//        _context = context;
//    }

//    public async Task<List<NPCPotential>> GetAllNPCPotentialsAsync()
//    {
//        return await _context.NPCPotentials.ToListAsync();
//    }

//    public async Task<NPCPotential?> GetNPCPotentialByIdAsync(Guid npcId, Guid potentialId)
//    {
//        return await _context.NPCPotentials
//            .FirstOrDefaultAsync(np => np.NPCId == npcId && np.PotentialId == potentialId);
//    }

//    public async Task<NPCPotential> CreateNPCPotentialAsync(NPCPotential npcPotential)
//    {
//        _context.NPCPotentials.Add(npcPotential);
//        await _context.SaveChangesAsync();
//        return npcPotential;
//    }

//    public async Task<NPCPotential?> UpdateNPCPotentialAsync(Guid npcId, Guid potentialId, NPCPotential npcPotential)
//    {
//        var existingNPCPotential = await _context.NPCPotentials
//            .FirstOrDefaultAsync(np => np.NPCId == npcId && np.PotentialId == potentialId);

//        if (existingNPCPotential is null) return null;

//        _context.Entry(existingNPCPotential).CurrentValues.SetValues(npcPotential);
//        await _context.SaveChangesAsync();
//        return existingNPCPotential;
//    }

//    public async Task<bool> DeleteNPCPotentialAsync(Guid npcId, Guid potentialId)
//    {
//        var npcPotential = await _context.NPCPotentials
//            .FirstOrDefaultAsync(np => np.NPCId == npcId && np.PotentialId == potentialId);

//        if (npcPotential is null) return false;

//        _context.NPCPotentials.Remove(npcPotential);
//        await _context.SaveChangesAsync();
//        return true;
//    }
//}
