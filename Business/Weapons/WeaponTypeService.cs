using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs.Weapons;
using Domain.Weapons;
using Microsoft.EntityFrameworkCore;

namespace Business.Weapons;
public interface IWeaponTypeService
{
    Task<IEnumerable<WeaponTypeDto>> GetAllWeaponTypesAsync();
    Task<WeaponTypeDto?> GetWeaponTypeByIdAsync(Guid id);
    Task<WeaponTypeDto?> CreateWeaponTypeAsync(WeaponTypeCreateDto weaponTypeCreate);
    Task<bool> UpdateWeaponTypeAsync(WeaponTypeDto weaponType);
    Task<bool> DeleteWeaponTypeAsync(Guid id);
}

public class WeaponTypeService : IWeaponTypeService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public WeaponTypeService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<WeaponTypeDto>> GetAllWeaponTypesAsync()
    {
        var weaponTypes = await _context.WeaponTypes.ToListAsync();
        return _mapper.Map<IEnumerable<WeaponTypeDto>>(weaponTypes);
    }

    public async Task<WeaponTypeDto?> GetWeaponTypeByIdAsync(Guid id)
    {
        var weaponType = await _context.WeaponTypes.FindAsync(id);
        return weaponType is null ? null : _mapper.Map<WeaponTypeDto>(weaponType);
    }

    public async Task<WeaponTypeDto?> CreateWeaponTypeAsync(WeaponTypeCreateDto weaponTypeCreate)
    {
        var weaponType = _mapper.Map<WeaponType>(weaponTypeCreate);
        _context.WeaponTypes.Add(weaponType);
        await _context.SaveChangesAsync();
        return _mapper.Map<WeaponTypeDto>(weaponType);
    }

    public async Task<bool> UpdateWeaponTypeAsync(WeaponTypeDto weaponTypeDto)
    {
        var existingWeaponType = await _context.WeaponTypes.FindAsync(weaponTypeDto.Id);
        if (existingWeaponType is null)
            return false;

        _mapper.Map(weaponTypeDto, existingWeaponType);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteWeaponTypeAsync(Guid id)
    {
        var weaponType = await _context.WeaponTypes.FindAsync(id);
        if (weaponType is null)
            return false;

        _context.WeaponTypes.Remove(weaponType);
        await _context.SaveChangesAsync();
        return true;
    }
}