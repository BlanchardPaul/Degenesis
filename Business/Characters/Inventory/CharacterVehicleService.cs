using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs.Characters.CRUD.Inventory;
using Domain.Characters.Inventory;
using Microsoft.EntityFrameworkCore;

namespace Business.Characters.Inventory;


public interface ICharacterVehicleService
{
    Task<List<CharacterVehicleDto>> GetByCharacterIdAsync(Guid characterId);
    Task<CharacterVehicleDto?> CreateAsync(CharacterVehicleCreateDto characterVehicle);
    Task<bool> UpdateAsync(CharacterVehicleDto characterVehicle);
    Task<bool> DeleteAsync(Guid id);
}

public class CharacterVehicleService : ICharacterVehicleService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CharacterVehicleService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<CharacterVehicleDto>> GetByCharacterIdAsync(Guid characterId)
    {
        var characterVehicles =
            await _context.CharacterVehicles
            .Where(cv => cv.CharacterId == characterId)
            .Include(cv => cv.Vehicle)
                .ThenInclude(v => v.VehicleType)
            .Include(cv => cv.Vehicle)
                .ThenInclude(v => v.Cult)
            .ToListAsync();
        return _mapper.Map<List<CharacterVehicleDto>>(characterVehicles);
    }

    public async Task<CharacterVehicleDto?> CreateAsync(CharacterVehicleCreateDto characterVehicleCreate)
    {
        try
        {
            var characterVehicle = _mapper.Map<CharacterVehicle>(characterVehicleCreate);
            characterVehicle.Character = await _context.Characters.FindAsync(characterVehicleCreate.CharacterId)
                ?? throw new Exception("Character not found");
            characterVehicle.Vehicle = await _context.Vehicles.FindAsync(characterVehicleCreate.VehicleId)
                ?? throw new Exception("Vehicle not found");
            // Set the Slots, BodyFlesh, StructureTrauma based on the Vehicle entity
            characterVehicle.Slots = characterVehicle.Vehicle.Slots;
            characterVehicle.BodyFlesh = characterVehicle.Vehicle.BodyFlesh;
            characterVehicle.StructureTrauma = characterVehicle.Vehicle.StructureTrauma;
            _context.CharacterVehicles.Add(characterVehicle);
            await _context.SaveChangesAsync();
            return _mapper.Map<CharacterVehicleDto>(characterVehicle);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<bool> UpdateAsync(CharacterVehicleDto characterVehicleDto)
    {
        try
        {
            var characterVehicle = await _context.CharacterVehicles.FindAsync(characterVehicleDto.Id);
            if (characterVehicle == null) return false;
            _mapper.Map(characterVehicleDto, characterVehicle);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        try
        {
            var characterVehicle = await _context.CharacterVehicles.FindAsync(id);
            if (characterVehicle == null) return false;
            _context.CharacterVehicles.Remove(characterVehicle);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
