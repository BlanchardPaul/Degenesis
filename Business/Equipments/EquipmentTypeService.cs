using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs.Equipments;
using Domain.Equipments;
using Microsoft.EntityFrameworkCore;

namespace Business.Equipments;
public interface IEquipmentTypeService
{
    Task<List<EquipmentTypeDto>> GetAllEquipmentTypesAsync();
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

    public async Task<List<EquipmentTypeDto>> GetAllEquipmentTypesAsync()
    {
        var equipmentTypes = await _context.EquipmentTypes.ToListAsync();
        return _mapper.Map<List<EquipmentTypeDto>>(equipmentTypes);
    }

    public async Task<EquipmentTypeDto?> GetEquipmentTypeByIdAsync(Guid id)
    {
        try
        {
            var equipmentType = await _context.EquipmentTypes
                .FirstOrDefaultAsync(e => e.Id == id)
                ?? throw new Exception("EquipmentType not found");
            return _mapper.Map<EquipmentTypeDto>(equipmentType);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<EquipmentTypeDto?> CreateEquipmentTypeAsync(EquipmentTypeCreateDto equipmentTypeCreate)
    {
        try
        {
            var equipmentType = _mapper.Map<EquipmentType>(equipmentTypeCreate);
            _context.EquipmentTypes.Add(equipmentType);
            await _context.SaveChangesAsync();
            return _mapper.Map<EquipmentTypeDto>(equipmentType);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<bool> UpdateEquipmentTypeAsync(EquipmentTypeDto equipmentTypeDto)
    {
        try
        {
            var existingEquipmentType = await _context.EquipmentTypes
                .FirstOrDefaultAsync(e => e.Id == equipmentTypeDto.Id)
                ?? throw new Exception("EquipmentType not found");

            _mapper.Map(equipmentTypeDto, existingEquipmentType);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> DeleteEquipmentTypeAsync(Guid id)
    {
        try
        {
            var existingEquipmentType = await _context.EquipmentTypes
                .FirstOrDefaultAsync(e => e.Id == id)
                ?? throw new Exception("EquipmentType not found");
            if (existingEquipmentType is null)
                return false;

            _context.EquipmentTypes.Remove(existingEquipmentType);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}