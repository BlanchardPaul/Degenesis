using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Characters.CRUD;
using Degenesis.Shared.DTOs.Weapons;
using Domain.Burns;
using Domain.Weapons;
using Microsoft.EntityFrameworkCore;

namespace Business.Weapons;
public interface IWeaponTypeService
{
    Task<Result<List<WeaponTypeDto>>> GetAllWeaponTypesAsync();
    Task<Result<WeaponTypeDto>> GetWeaponTypeByIdAsync(Guid id);
    Task<Result<object>> CreateWeaponTypeAsync(WeaponTypeCreateDto weaponTypeCreate);
    Task<Result<object>> UpdateWeaponTypeAsync(WeaponTypeDto weaponType);
    Task<Result<object>> DeleteWeaponTypeAsync(Guid id);
}

public class WeaponTypeService(ApplicationDbContext context, IMapper mapper) : IWeaponTypeService
{
    private readonly ApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<List<WeaponTypeDto>>> GetAllWeaponTypesAsync()
    {
        try
        {
            var weaponTypes = await _context.WeaponTypes.OrderBy(w => w.Name).ToListAsync();
            return new Result<List<WeaponTypeDto>> { Value = _mapper.Map<List<WeaponTypeDto>>(weaponTypes) };
        }
        catch (Exception)
        {
            return new Result<List<WeaponTypeDto>> { IsError = true, Error = "A server error occurred" };
        }

    }

    public async Task<Result<WeaponTypeDto>> GetWeaponTypeByIdAsync(Guid id)
    {
        try
        {
            var weaponType = await _context.WeaponTypes.FindAsync(id);
            if (weaponType is null)
                return new Result<WeaponTypeDto> { IsError = true, Error = "Weapon type not found" };
            return new Result<WeaponTypeDto> { Value = _mapper.Map<WeaponTypeDto>(weaponType) };
        }
        catch (Exception)
        {
            return new Result<WeaponTypeDto> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> CreateWeaponTypeAsync(WeaponTypeCreateDto weaponTypeCreate)
    {
        try
        {
            var weaponType = _mapper.Map<WeaponType>(weaponTypeCreate);
            _context.WeaponTypes.Add(weaponType);
            await _context.SaveChangesAsync();
            return new Result<object> { Value = _mapper.Map<WeaponTypeDto>(weaponType) };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> UpdateWeaponTypeAsync(WeaponTypeDto weaponTypeDto)
    {
        try
        {
            var existingWeaponType = await _context.WeaponTypes.FindAsync(weaponTypeDto.Id);
            if (existingWeaponType is null)
                return new Result<object> { IsError = true, Error = "Weapon type not found" };

            _mapper.Map(weaponTypeDto, existingWeaponType);
            await _context.SaveChangesAsync();
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> DeleteWeaponTypeAsync(Guid id)
    {
        try
        {
            var weaponType = await _context.WeaponTypes.FindAsync(id);
            if (weaponType is null)
                return new Result<object> { IsError = true, Error = "Weapon type not found" };

            _context.WeaponTypes.Remove(weaponType);
            await _context.SaveChangesAsync();
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }
}