//using DataAccessLayer;
//using Domain.NPCs;
//using Microsoft.EntityFrameworkCore;

//namespace Business.NPCs;
//public interface INPCAttributeService
//{
//    Task<List<NPCAttribute>> GetAllNPCAttributesAsync();
//    Task<NPCAttribute?> GetNPCAttributeByIdAsync(Guid npcId, Guid attributeId);
//    Task<NPCAttribute> CreateNPCAttributeAsync(NPCAttribute npcAttribute);
//    Task<NPCAttribute?> UpdateNPCAttributeAsync(Guid npcId, Guid attributeId, NPCAttribute npcAttribute);
//    Task<bool> DeleteNPCAttributeAsync(Guid npcId, Guid attributeId);
//}

//public class NPCAttributeService : INPCAttributeService
//{
//    private readonly ApplicationDbContext _context;

//    public NPCAttributeService(ApplicationDbContext context)
//    {
//        _context = context;
//    }

//    public async Task<List<NPCAttribute>> GetAllNPCAttributesAsync()
//    {
//        return await _context.NPCAttributes.ToListAsync();
//    }

//    public async Task<NPCAttribute?> GetNPCAttributeByIdAsync(Guid npcId, Guid attributeId)
//    {
//        return await _context.NPCAttributes
//            .FirstOrDefaultAsync(na => na.NPCId == npcId && na.AttributeId == attributeId);
//    }

//    public async Task<NPCAttribute> CreateNPCAttributeAsync(NPCAttribute npcAttribute)
//    {
//        _context.NPCAttributes.Add(npcAttribute);
//        await _context.SaveChangesAsync();
//        return npcAttribute;
//    }

//    public async Task<NPCAttribute?> UpdateNPCAttributeAsync(Guid npcId, Guid attributeId, NPCAttribute npcAttribute)
//    {
//        var existingNPCAttribute = await _context.NPCAttributes
//            .FirstOrDefaultAsync(na => na.NPCId == npcId && na.AttributeId == attributeId);

//        if (existingNPCAttribute is null) return null;

//        _context.Entry(existingNPCAttribute).CurrentValues.SetValues(npcAttribute);
//        await _context.SaveChangesAsync();
//        return existingNPCAttribute;
//    }

//    public async Task<bool> DeleteNPCAttributeAsync(Guid npcId, Guid attributeId)
//    {
//        var npcAttribute = await _context.NPCAttributes
//            .FirstOrDefaultAsync(na => na.NPCId == npcId && na.AttributeId == attributeId);

//        if (npcAttribute is null) return false;

//        _context.NPCAttributes.Remove(npcAttribute);
//        await _context.SaveChangesAsync();
//        return true;
//    }
//}
