using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs.Weapons;
using Domain.Weapons;
using Microsoft.EntityFrameworkCore;

namespace Business.Weapons;
public interface IWeaponTypeService
{
    Task<List<WeaponTypeDto>> GetAllWeaponTypesAsync();
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

    public async Task<List<WeaponTypeDto>> GetAllWeaponTypesAsync()
    {
        var weaponTypes = await _context.WeaponTypes.ToListAsync();
        return _mapper.Map<List<WeaponTypeDto>>(weaponTypes);
    }

    public async Task<WeaponTypeDto?> GetWeaponTypeByIdAsync(Guid id)
    {
        try
        {
            var weaponType = await _context.WeaponTypes.FindAsync(id)
                ?? throw new Exception("WeaponType not found");
            return _mapper.Map<WeaponTypeDto>(weaponType);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<WeaponTypeDto?> CreateWeaponTypeAsync(WeaponTypeCreateDto weaponTypeCreate)
    {
        try
        {
            var weaponType = _mapper.Map<WeaponType>(weaponTypeCreate);
            _context.WeaponTypes.Add(weaponType);
            await _context.SaveChangesAsync();
            return _mapper.Map<WeaponTypeDto>(weaponType);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<bool> UpdateWeaponTypeAsync(WeaponTypeDto weaponTypeDto)
    {
        try
        {
            var existingWeaponType = await _context.WeaponTypes.FindAsync(weaponTypeDto.Id)
                ?? throw new Exception("WeaponType not found");

            _mapper.Map(weaponTypeDto, existingWeaponType);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> DeleteWeaponTypeAsync(Guid id)
    {
        try
        {
            var weaponType = await _context.WeaponTypes.FindAsync(id)
                ?? throw new Exception("WeaponType not found");

            _context.WeaponTypes.Remove(weaponType);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}