using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs.Equipments;
using Domain.Equipments;
using Microsoft.EntityFrameworkCore;

namespace Business.Equipments; public interface IEquipmentService
{
    Task<IEnumerable<EquipmentDto>> GetAllEquipmentsAsync();
    Task<EquipmentDto?> GetEquipmentByIdAsync(Guid id);
    Task<EquipmentDto?> CreateEquipmentAsync(EquipmentCreateDto equipmentCreate);
    Task<bool> UpdateEquipmentAsync(EquipmentDto equipment);
    Task<bool> DeleteEquipmentAsync(Guid id);
}

public class EquipmentService : IEquipmentService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public EquipmentService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<EquipmentDto>> GetAllEquipmentsAsync()
    {
        var equipments = await _context.Equipments
            .Include(e => e.EquipmentType)
            .ToListAsync();
        return _mapper.Map<IEnumerable<EquipmentDto>>(equipments);
    }

    public async Task<EquipmentDto?> GetEquipmentByIdAsync(Guid id)
    {
        var equipment = await _context.Equipments
            .Include(e => e.EquipmentType)
            .FirstOrDefaultAsync(e => e.Id == id);

        return equipment is null ? null : _mapper.Map<EquipmentDto>(equipment);
    }

    public async Task<EquipmentDto?> CreateEquipmentAsync(EquipmentCreateDto equipmentCreate)
    {
        try
        {
            var equipment = _mapper.Map<Equipment>(equipmentCreate);
            equipment.EquipmentType = await _context.EquipmentTypes.FirstAsync(et => et.Id == equipmentCreate.EquipmentTypeId);

            _context.Equipments.Add(equipment);
            await _context.SaveChangesAsync();
            return _mapper.Map<EquipmentDto>(equipment);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<bool> UpdateEquipmentAsync(EquipmentDto equipmentDto)
    {
        try
        {
            var existingEquipment = await _context.Equipments
                .Include(e => e.EquipmentType)
                .FirstOrDefaultAsync(e => e.Id == equipmentDto.Id);

            if (existingEquipment is null || equipmentDto.EquipmentType is null)
                return false;

            _mapper.Map(equipmentDto, existingEquipment);
            existingEquipment.EquipmentType = await _context.EquipmentTypes.FirstAsync(et => et.Id == equipmentDto.EquipmentType.Id);

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> DeleteEquipmentAsync(Guid id)
    {
        try
        {
            var equipment = await _context.Equipments.FindAsync(id);
            if (equipment is null)
                return false;

            _context.Equipments.Remove(equipment);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}