//using DataAccessLayer;
//using Domain.Protections;
//using Microsoft.EntityFrameworkCore;

//namespace Business.Protections;
//public interface INPCProtectionService
//{
//    Task<List<NPCProtection>> GetAllNPCProtectionsAsync();
//    Task<NPCProtection?> GetNPCProtectionByIdAsync(Guid id);
//    Task<NPCProtection> CreateNPCProtectionAsync(NPCProtection npcProtection);
//    Task<NPCProtection?> UpdateNPCProtectionAsync(Guid id, NPCProtection npcProtection);
//    Task<bool> DeleteNPCProtectionAsync(Guid id);
//}

//public class NPCProtectionService : INPCProtectionService
//{
//    private readonly ApplicationDbContext _context;

//    public NPCProtectionService(ApplicationDbContext context)
//    {
//        _context = context;
//    }

//    public async Task<List<NPCProtection>> GetAllNPCProtectionsAsync()
//    {
//        return await _context.NPCProtections.ToListAsync();
//    }

//    public async Task<NPCProtection?> GetNPCProtectionByIdAsync(Guid id)
//    {
//        return await _context.NPCProtections.FindAsync(id);
//    }

//    public async Task<NPCProtection> CreateNPCProtectionAsync(NPCProtection npcProtection)
//    {
//        _context.NPCProtections.Add(npcProtection);
//        await _context.SaveChangesAsync();
//        return npcProtection;
//    }

//    public async Task<NPCProtection?> UpdateNPCProtectionAsync(Guid id, NPCProtection npcProtection)
//    {
//        var existingNPCProtection = await _context.NPCProtections.FindAsync(id);

//        if (existingNPCProtection is null)
//        {
//            return null;
//        }

//        existingNPCProtection = npcProtection;

//        await _context.SaveChangesAsync();
//        return existingNPCProtection;
//    }

//    public async Task<bool> DeleteNPCProtectionAsync(Guid id)
//    {
//        var existingNPCProtection = await _context.NPCProtections.FindAsync(id);

//        if (existingNPCProtection is null)
//        {
//            return false;
//        }

//        _context.NPCProtections.Remove(existingNPCProtection);
//        await _context.SaveChangesAsync();
//        return true;
//    }
//}
