using DataAccessLayer;
using Domain.Equipments;
using Microsoft.EntityFrameworkCore;

namespace Business.Equipments;
public interface IEquipmentTypeService
{
    Task<List<EquipmentType>> GetAllEquipmentTypesAsync();
    Task<EquipmentType?> GetEquipmentTypeByIdAsync(Guid id);
    Task<EquipmentType> CreateEquipmentTypeAsync(EquipmentType equipmentType);
    Task<EquipmentType?> UpdateEquipmentTypeAsync(Guid id, EquipmentType equipmentType);
    Task<bool> DeleteEquipmentTypeAsync(Guid id);
}

public class EquipmentTypeService : IEquipmentTypeService
{
    private readonly ApplicationDbContext _context;

    public EquipmentTypeService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<EquipmentType>> GetAllEquipmentTypesAsync()
    {
        return await _context.EquipmentTypes.ToListAsync();
    }

    public async Task<EquipmentType?> GetEquipmentTypeByIdAsync(Guid id)
    {
        return await _context.EquipmentTypes.FindAsync(id);
    }

    public async Task<EquipmentType> CreateEquipmentTypeAsync(EquipmentType equipmentType)
    {
        _context.EquipmentTypes.Add(equipmentType);
        await _context.SaveChangesAsync();
        return equipmentType;
    }

    public async Task<EquipmentType?> UpdateEquipmentTypeAsync(Guid id, EquipmentType equipmentType)
    {
        var existingEquipmentType = await _context.EquipmentTypes.FindAsync(id);

        if (existingEquipmentType == null)
        {
            return null;
        }

        existingEquipmentType = equipmentType;

        await _context.SaveChangesAsync();
        return existingEquipmentType;
    }

    public async Task<bool> DeleteEquipmentTypeAsync(Guid id)
    {
        var existingEquipmentType = await _context.EquipmentTypes.FindAsync(id);

        if (existingEquipmentType == null)
        {
            return false;
        }

        _context.EquipmentTypes.Remove(existingEquipmentType);
        await _context.SaveChangesAsync();
        return true;
    }
}
