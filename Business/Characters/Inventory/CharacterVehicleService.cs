using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Characters.CRUD.Inventory;
using Domain.Characters.Inventory;
using Microsoft.EntityFrameworkCore;

namespace Business.Characters.Inventory;

public interface ICharacterVehicleService
{
    Task<Result<List<CharacterVehicleDto>>> GetByCharacterIdAsync(Guid characterId);
    Task<Result<object>> CreateAsync(CharacterVehicleCreateDto characterVehicle);
    Task<Result<object>> UpdateAsync(CharacterVehicleDto characterVehicle);
    Task<Result<object>> DeleteAsync(Guid id);
}

public class CharacterVehicleService(ApplicationDbContext context, IMapper mapper) : ICharacterVehicleService
{
    private readonly ApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<List<CharacterVehicleDto>>> GetByCharacterIdAsync(Guid characterId)
    {
        try
        {
            var characterVehicles =
                await _context.CharacterVehicles
                .Where(cv => cv.CharacterId == characterId)
                .Include(cv => cv.Vehicle)
                    .ThenInclude(v => v.VehicleType)
                .Include(cv => cv.Vehicle)
                    .ThenInclude(v => v.Cult)
                .ToListAsync();

            return new Result<List<CharacterVehicleDto>> { Value = _mapper.Map<List<CharacterVehicleDto>>(characterVehicles) };
        }
        catch (Exception)
        {
            return new Result<List<CharacterVehicleDto>> { IsError = true, Error = "A server error occurred" };
        }

    }

    public async Task<Result<object>> CreateAsync(CharacterVehicleCreateDto characterVehicleCreate)
    {
        try
        {
            var characterVehicle = _mapper.Map<CharacterVehicle>(characterVehicleCreate);

            var existingCharacter = await _context.Characters.FindAsync(characterVehicle.CharacterId);
            if (existingCharacter is null)
                return new Result<object> { IsError = true, Error = "Character not found" };

            var existingVehicle = await _context.Vehicles.FindAsync(characterVehicle.VehicleId);
            if (existingVehicle is null)
                return new Result<object> { IsError = true, Error = "Vehicle not found" };

            characterVehicle.Character = existingCharacter;
            characterVehicle.Vehicle = existingVehicle;

            // Set the Slots, BodyFlesh, StructureTrauma based on the Vehicle entity
            characterVehicle.Slots = characterVehicle.Vehicle.Slots;
            characterVehicle.BodyFlesh = characterVehicle.Vehicle.BodyFlesh;
            characterVehicle.StructureTrauma = characterVehicle.Vehicle.StructureTrauma;

            _context.CharacterVehicles.Add(characterVehicle);
            await _context.SaveChangesAsync();
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> UpdateAsync(CharacterVehicleDto characterVehicleDto)
    {
        try
        {
            var characterVehicle = await _context.CharacterVehicles.FindAsync(characterVehicleDto.Id);
            if (characterVehicle is null) 
                return new Result<object> { IsError = true, Error = "CharacterVehicle not found" };
            _mapper.Map(characterVehicleDto, characterVehicle);
            await _context.SaveChangesAsync();
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> DeleteAsync(Guid id)
    {
        try
        {
            var characterVehicle = await _context.CharacterVehicles.FindAsync(id);
            if (characterVehicle is null)
                return new Result<object> { IsError = true, Error = "CharacterVehicle not found" };

            _context.CharacterVehicles.Remove(characterVehicle);
            await _context.SaveChangesAsync();
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }
}
