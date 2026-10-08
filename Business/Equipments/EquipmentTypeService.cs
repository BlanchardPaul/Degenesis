using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Equipments;
using Domain.Equipments;
using Microsoft.EntityFrameworkCore;

namespace Business.Equipments;
public interface IEquipmentTypeService
{
    Task<Result<List<EquipmentTypeDto>>> GetAllEquipmentTypesAsync();
    Task<Result<EquipmentTypeDto>> GetEquipmentTypeByIdAsync(Guid id);
    Task<Result<object>> CreateEquipmentTypeAsync(EquipmentTypeCreateDto equipmentTypeCreate);
    Task<Result<object>> UpdateEquipmentTypeAsync(EquipmentTypeDto equipmentType);
    Task<Result<object>> DeleteEquipmentTypeAsync(Guid id);
}

public class EquipmentTypeService(ApplicationDbContext context, IMapper mapper) : IEquipmentTypeService
{
    private readonly ApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<List<EquipmentTypeDto>>> GetAllEquipmentTypesAsync()
    {
        try
        {
            var equipmentTypes = await _context.EquipmentTypes.OrderBy(e => e.Name).ToListAsync();
            return new Result<List<EquipmentTypeDto>> { Value = _mapper.Map<List<EquipmentTypeDto>>(equipmentTypes) };
        }
        catch (Exception)
        {
            return new Result<List<EquipmentTypeDto>> { IsError = true, Error = "A server error occurred" };
        }

    }

    public async Task<Result<EquipmentTypeDto>> GetEquipmentTypeByIdAsync(Guid id)
    {
        try
        {
            var equipmentType = await _context.EquipmentTypes
                .FirstOrDefaultAsync(e => e.Id == id);
            if(equipmentType is null)
                return new Result<EquipmentTypeDto> { IsError = true, Error = "EquipmentType not found" };

            return new Result<EquipmentTypeDto> { Value = _mapper.Map<EquipmentTypeDto>(equipmentType) };
        }
        catch (Exception)
        {
            return new Result<EquipmentTypeDto> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> CreateEquipmentTypeAsync(EquipmentTypeCreateDto equipmentTypeCreate)
    {
        try
        {
            var equipmentType = _mapper.Map<EquipmentType>(equipmentTypeCreate);
            _context.EquipmentTypes.Add(equipmentType);
            await _context.SaveChangesAsync();
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> UpdateEquipmentTypeAsync(EquipmentTypeDto equipmentTypeDto)
    {
        try
        {
            var existingEquipmentType = await _context.EquipmentTypes
                .FirstOrDefaultAsync(e => e.Id == equipmentTypeDto.Id);
            if (existingEquipmentType is null)
                return new Result<object> { IsError = true, Error = "EquipmentType not found" };

            _mapper.Map(equipmentTypeDto, existingEquipmentType);
            await _context.SaveChangesAsync();
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> DeleteEquipmentTypeAsync(Guid id)
    {
        try
        {
            var existingEquipmentType = await _context.EquipmentTypes
                .FirstOrDefaultAsync(e => e.Id == id);
            if (existingEquipmentType is null)
                return new Result<object> { IsError = true, Error = "EquipmentType not found" };

            _context.EquipmentTypes.Remove(existingEquipmentType);
            await _context.SaveChangesAsync();
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }
}