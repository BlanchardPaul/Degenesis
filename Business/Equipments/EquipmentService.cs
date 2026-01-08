using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs.Equipments;
using Domain.Equipments;
using Microsoft.EntityFrameworkCore;

namespace Business.Equipments; 
public interface IEquipmentService
{
    Task<List<EquipmentDto>> GetAllEquipmentsAsync();
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

    public async Task<List<EquipmentDto>> GetAllEquipmentsAsync()
    {
        var equipments = await _context.Equipments
            .Include(e => e.EquipmentType)
            .Include(e => e.Cults)
            .ToListAsync();
        return _mapper.Map<List<EquipmentDto>>(equipments);
    }

    public async Task<EquipmentDto?> GetEquipmentByIdAsync(Guid id)
    {
        try
        {
            var equipment = await _context.Equipments
                .Include(e => e.EquipmentType)
                .Include(e => e.Cults)
                .FirstOrDefaultAsync(e => e.Id == id) 
                ?? throw new Exception("Equipment not found");
            return _mapper.Map<EquipmentDto>(equipment);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<EquipmentDto?> CreateEquipmentAsync(EquipmentCreateDto equipmentCreate)
    {
        try
        {
            var equipment = _mapper.Map<Equipment>(equipmentCreate);
            equipment.EquipmentType = await _context.EquipmentTypes
                .FirstOrDefaultAsync(et => et.Id == equipmentCreate.EquipmentTypeId) 
                ?? throw new Exception("EquipmentType not found");

            foreach (var cultDto in equipmentCreate.Cults)
            {
                var cult = await _context.Cults
                    .FirstOrDefaultAsync(c => c.Id == cultDto.Id) ?? throw new Exception("Cult not found");
                equipment.Cults.Add(cult);
            }

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
                .FirstOrDefaultAsync(e => e.Id == equipmentDto.Id)
                ?? throw new Exception("Equipment not found");

            _mapper.Map(equipmentDto, existingEquipment);
            existingEquipment.EquipmentType = await _context.EquipmentTypes
                .FirstOrDefaultAsync(et => et.Id == equipmentDto.EquipmentType.Id) 
                ?? throw new Exception("EquipmentType not found");

            existingEquipment.Cults.Clear();
            foreach (var cultDto in equipmentDto.Cults)
            {
                var cult = await _context.Cults
                    .FirstOrDefaultAsync(c => c.Id == cultDto.Id) ?? throw new Exception("Cult not found");
                existingEquipment.Cults.Add(cult);
            }

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
            var equipment = await _context.Equipments
                .FirstOrDefaultAsync(e => e.Id == id)
                ?? throw new Exception("Equipment not found");
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