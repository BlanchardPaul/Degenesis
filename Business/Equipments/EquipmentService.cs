using DataAccessLayer;
using Domain.Equipments;
using Microsoft.EntityFrameworkCore;

namespace Business.Equipments;
public interface IEquipmentService
{
    Task<List<Equipment>> GetAllEquipmentsAsync();
    Task<Equipment?> GetEquipmentByIdAsync(Guid id);
    Task<Equipment> CreateEquipmentAsync(Equipment equipment);
    Task<Equipment?> UpdateEquipmentAsync(Guid id, Equipment equipment);
    Task<bool> DeleteEquipmentAsync(Guid id);
}
public class EquipmentService : IEquipmentService
{
    private readonly ApplicationDbContext _context;

    public EquipmentService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Equipment>> GetAllEquipmentsAsync()
    {
        return await _context.Equipments
            .Include(e => e.EquipmentType)
            .ToListAsync();
    }

    public async Task<Equipment?> GetEquipmentByIdAsync(Guid id)
    {
        return await _context.Equipments
            .Include(e => e.EquipmentType)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<Equipment> CreateEquipmentAsync(Equipment equipment)
    {
        _context.Equipments.Add(equipment);
        await _context.SaveChangesAsync();
        return equipment;
    }

    public async Task<Equipment?> UpdateEquipmentAsync(Guid id, Equipment equipment)
    {
        var existingEquipment = await _context.Equipments.FindAsync(id);

        if (existingEquipment == null)
        {
            return null;
        }

        existingEquipment = equipment;

        await _context.SaveChangesAsync();
        return existingEquipment;
    }

    public async Task<bool> DeleteEquipmentAsync(Guid id)
    {
        var existingEquipment = await _context.Equipments.FindAsync(id);

        if (existingEquipment == null)
        {
            return false;
        }

        _context.Equipments.Remove(existingEquipment);
        await _context.SaveChangesAsync();
        return true;
    }
}
