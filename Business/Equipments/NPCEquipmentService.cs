using DataAccessLayer;
using Domain.Equipments;
using Microsoft.EntityFrameworkCore;

namespace Business.Equipments;
public interface INPCEquipmentService
{
    Task<List<NPCEquipment>> GetAllNPCEquipmentsAsync();
    Task<NPCEquipment?> GetNPCEquipmentByIdAsync(Guid id);
    Task<NPCEquipment> CreateNPCEquipmentAsync(NPCEquipment npcEquipment);
    Task<NPCEquipment?> UpdateNPCEquipmentAsync(Guid id, NPCEquipment npcEquipment);
    Task<bool> DeleteNPCEquipmentAsync(Guid id);
}
public class NPCEquipmentService : INPCEquipmentService
{
    private readonly ApplicationDbContext _context;

    public NPCEquipmentService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<NPCEquipment>> GetAllNPCEquipmentsAsync()
    {
        return await _context.NPCEquipments.ToListAsync();
    }

    public async Task<NPCEquipment?> GetNPCEquipmentByIdAsync(Guid id)
    {
        return await _context.NPCEquipments.FindAsync(id);
    }

    public async Task<NPCEquipment> CreateNPCEquipmentAsync(NPCEquipment npcEquipment)
    {
        _context.NPCEquipments.Add(npcEquipment);
        await _context.SaveChangesAsync();
        return npcEquipment;
    }

    public async Task<NPCEquipment?> UpdateNPCEquipmentAsync(Guid id, NPCEquipment npcEquipment)
    {
        var existingNPCEquipment = await _context.NPCEquipments.FindAsync(id);

        if (existingNPCEquipment is null)
        {
            return null;
        }

        existingNPCEquipment = npcEquipment;

        await _context.SaveChangesAsync();
        return existingNPCEquipment;
    }

    public async Task<bool> DeleteNPCEquipmentAsync(Guid id)
    {
        var existingNPCEquipment = await _context.NPCEquipments.FindAsync(id);

        if (existingNPCEquipment is null)
        {
            return false;
        }

        _context.NPCEquipments.Remove(existingNPCEquipment);
        await _context.SaveChangesAsync();
        return true;
    }
}
