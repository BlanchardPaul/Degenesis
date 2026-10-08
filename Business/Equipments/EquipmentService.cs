using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Equipments;
using Domain.Equipments;
using Microsoft.EntityFrameworkCore;

namespace Business.Equipments; 
public interface IEquipmentService
{
    Task<Result<List<EquipmentDto>>> GetAllEquipmentsAsync();
    Task<Result<EquipmentDto>> GetEquipmentByIdAsync(Guid id);
    Task<Result<object>> CreateEquipmentAsync(EquipmentCreateDto equipmentCreate);
    Task<Result<object>> UpdateEquipmentAsync(EquipmentDto equipment);
    Task<Result<object>> DeleteEquipmentAsync(Guid id);
}

public class EquipmentService(ApplicationDbContext context, IMapper mapper) : IEquipmentService
{
    private readonly ApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<List<EquipmentDto>>> GetAllEquipmentsAsync()
    {
        try
        {
            var equipments = await _context.Equipments
                .Include(e => e.EquipmentType)
                .Include(e => e.Cults)
                .OrderBy(e => e.Name)
                .ToListAsync();
            return new Result<List<EquipmentDto>> { Value = _mapper.Map<List<EquipmentDto>>(equipments) };
        }
        catch (Exception)
        {
            return new Result<List<EquipmentDto>> { IsError = true, Error = "A server error occurred" };
        }

    }

    public async Task<Result<EquipmentDto>> GetEquipmentByIdAsync(Guid id)
    {
        try
        {
            var equipment = await _context.Equipments
                .Include(e => e.EquipmentType)
                .Include(e => e.Cults)
                .FirstOrDefaultAsync(e => e.Id == id);
            if (equipment is null)
                return new Result<EquipmentDto> { IsError = true, Error = "Equipment not found" };

            return new Result<EquipmentDto> { Value = _mapper.Map<EquipmentDto>(equipment) };
        }
        catch (Exception)
        {
            return new Result<EquipmentDto> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> CreateEquipmentAsync(EquipmentCreateDto equipmentCreate)
    {
        try
        {
            var equipment = _mapper.Map<Equipment>(equipmentCreate);

            var equipmentType = await _context.EquipmentTypes
                .FirstOrDefaultAsync(et => et.Id == equipmentCreate.EquipmentTypeId);
            if (equipmentType is null)
                return new Result<object> { IsError = true, Error = "EquipmentType not found" };
            equipment.EquipmentType = equipmentType;

            foreach (var cultDto in equipmentCreate.Cults)
            {
                var cult = await _context.Cults
                    .FirstOrDefaultAsync(c => c.Id == cultDto.Id);
                if (cult is null)
                    return new Result<object> { IsError = true, Error = "Cult not found" };
                equipment.Cults.Add(cult);
            }

            _context.Equipments.Add(equipment);
            await _context.SaveChangesAsync();
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> UpdateEquipmentAsync(EquipmentDto equipmentDto)
    {
        try
        {
            var existingEquipment = await _context.Equipments
                .Include(e => e.EquipmentType)
                .Include(e => e.Cults)
                .FirstOrDefaultAsync(e => e.Id == equipmentDto.Id);
            if (existingEquipment is null)
                return new Result<object> { IsError = true, Error = "Equipment not found" };

            _mapper.Map(equipmentDto, existingEquipment);

            var equipmentType = await _context.EquipmentTypes
                .FirstOrDefaultAsync(et => et.Id == equipmentDto.EquipmentType.Id);
            if (equipmentType is null)
                return new Result<object> { IsError = true, Error = "EquipmentType not found" };
            existingEquipment.EquipmentType = equipmentType;

            existingEquipment.Cults.Clear();
            foreach (var cultDto in equipmentDto.Cults)
            {
                var cult = await _context.Cults
                    .FirstOrDefaultAsync(c => c.Id == cultDto.Id);
                if (cult is null)
                    return new Result<object> { IsError = true, Error = "Cult not found" };

                existingEquipment.Cults.Add(cult);
            }

            await _context.SaveChangesAsync();
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> DeleteEquipmentAsync(Guid id)
    {
        try
        {
            var equipment = await _context.Equipments
                .FirstOrDefaultAsync(e => e.Id == id);
            if (equipment is null)
                return new Result<object> { IsError = true, Error = "Equipment not found" };

            _context.Equipments.Remove(equipment);
            await _context.SaveChangesAsync();
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }
}