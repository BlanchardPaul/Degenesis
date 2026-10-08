using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Characters.CRUD.Inventory;
using Domain.Characters.Inventory;
using Microsoft.EntityFrameworkCore;

namespace Business.Characters.Inventory;

public interface ICharacterEquipmentService
{
    Task<Result<CharacterEquipmentDto>> GetByIdAsync(Guid id);
    Task<Result<List<CharacterEquipmentDto>>> GetByCharacterIdAsync(Guid characterId);
    Task<Result<object>> CreateAsync(CharacterEquipmentCreateDto characterEquipment);
    Task<Result<object>> UpdateAsync(CharacterEquipmentDto characterEquipment);
    Task<Result<object>> DeleteAsync(Guid id);
}

public class CharacterEquipmentService(ApplicationDbContext context, IMapper mapper) : ICharacterEquipmentService
{
    private readonly ApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<CharacterEquipmentDto>> GetByIdAsync(Guid id)
    {
        try
        {
            var characterEquipment = await _context.CharacterEquipments
                .Include(ca => ca.Equipment)
                .FirstOrDefaultAsync(ca => ca.Id == id);
            if (characterEquipment is null)
                return new Result<CharacterEquipmentDto> { IsError = true, Error = "CharacterEquipment not found" };

            return new Result<CharacterEquipmentDto> { Value = _mapper.Map<CharacterEquipmentDto>(characterEquipment) };
        }
        catch (Exception)
        {
            return new Result<CharacterEquipmentDto> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<List<CharacterEquipmentDto>>> GetByCharacterIdAsync(Guid characterId)
    {
        try
        {
            var characterEquipments =
                await _context.CharacterEquipments
                .Where(ca => ca.CharacterId == characterId)
                .Include(ca => ca.Equipment)
                    .ThenInclude(e => e.EquipmentType)
                .ToListAsync();
            return new Result<List<CharacterEquipmentDto>> { Value = _mapper.Map<List<CharacterEquipmentDto>>(characterEquipments) };
        }
        catch (Exception)
        {
            return new Result<List<CharacterEquipmentDto>> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> CreateAsync(CharacterEquipmentCreateDto characterEquipmentCreate)
    {
        try
        {
            var characterEquipment = _mapper.Map<CharacterEquipment>(characterEquipmentCreate);

            var existingCharacter = await _context.Characters.FindAsync(characterEquipmentCreate.CharacterId);
            if (existingCharacter is null)
                return new Result<object> { IsError = true, Error = "Character not found" };

            var existingEquipment = await _context.Equipments.FindAsync(characterEquipmentCreate.EquipmentId);
            if (existingEquipment is null)
                return new Result<object> { IsError = true, Error = "Equipment not found" };

            characterEquipment.Character = existingCharacter;
            characterEquipment.Equipment = existingEquipment;

            _context.CharacterEquipments.Add(characterEquipment);
            await _context.SaveChangesAsync();
            return new Result<object>{ Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> UpdateAsync(CharacterEquipmentDto characterEquipment)
    {
        try
        {
            var existing = await _context.CharacterEquipments.FirstOrDefaultAsync(ca => ca.Id == characterEquipment.Id);
            if (existing is null)
                return new Result<object> { IsError = true, Error = "CharacterEquipment not found" };

            _mapper.Map(characterEquipment, existing);
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
            var characterEquipment = await _context.CharacterEquipments.FirstOrDefaultAsync(ca => ca.Id == id);
            if (characterEquipment is null)
                return new Result<object> { IsError = true, Error = "CharacterEquipment not found" };
            _context.CharacterEquipments.Remove(characterEquipment);
            await _context.SaveChangesAsync();
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }
}