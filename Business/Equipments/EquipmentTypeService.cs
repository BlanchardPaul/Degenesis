using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs.Equipments;
using Domain.Equipments;
using Microsoft.EntityFrameworkCore;

namespace Business.Equipments;
public interface IEquipmentTypeService
{
    Task<IEnumerable<EquipmentTypeDto>> GetAllEquipmentTypesAsync();
    Task<EquipmentTypeDto?> GetEquipmentTypeByIdAsync(Guid id);
    Task<EquipmentTypeDto?> CreateEquipmentTypeAsync(EquipmentTypeCreateDto equipmentTypeCreate);
    Task<bool> UpdateEquipmentTypeAsync(EquipmentTypeDto equipmentType);
    Task<bool> DeleteEquipmentTypeAsync(Guid id);
}

public class EquipmentTypeService : IEquipmentTypeService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public EquipmentTypeService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<EquipmentTypeDto>> GetAllEquipmentTypesAsync()
    {
        var equipmentTypes = await _context.EquipmentTypes.ToListAsync();
        return _mapper.Map<IEnumerable<EquipmentTypeDto>>(equipmentTypes);
    }

    public async Task<EquipmentTypeDto?> GetEquipmentTypeByIdAsync(Guid id)
    {
        var equipmentType = await _context.EquipmentTypes.FindAsync(id);
        return equipmentType is null ? null : _mapper.Map<EquipmentTypeDto>(equipmentType);
    }

    public async Task<EquipmentTypeDto?> CreateEquipmentTypeAsync(EquipmentTypeCreateDto equipmentTypeCreate)
    {
        var equipmentType = _mapper.Map<EquipmentType>(equipmentTypeCreate);
        _context.EquipmentTypes.Add(equipmentType);
        await _context.SaveChangesAsync();
        return _mapper.Map<EquipmentTypeDto>(equipmentType);
    }

    public async Task<bool> UpdateEquipmentTypeAsync(EquipmentTypeDto equipmentTypeDto)
    {
        var existingEquipmentType = await _context.EquipmentTypes.FindAsync(equipmentTypeDto.Id);

        if (existingEquipmentType == null)
            return false;

        _mapper.Map(equipmentTypeDto, existingEquipmentType);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteEquipmentTypeAsync(Guid id)
    {
        var existingEquipmentType = await _context.EquipmentTypes.FindAsync(id);
        if (existingEquipmentType == null)
            return false;

        _context.EquipmentTypes.Remove(existingEquipmentType);
        await _context.SaveChangesAsync();
        return true;
    }
}