using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Characters.CRUD.Inventory;
using Domain.Characters.Inventory;
using Microsoft.EntityFrameworkCore;

namespace Business.Characters.Inventory;

public interface ICharacterProtectionService
{
    Task<Result<List<CharacterProtectionDto>>> GetByCharacterIdAsync(Guid characterId);
    Task<Result<object>> CreateAsync(CharacterProtectionCreateDto characterProtection);
    Task<Result<object>> UpdateAsync(CharacterProtectionDto characterProtection);
    Task<Result<object>> DeleteAsync(Guid id);
}

public class CharacterProtectionService(ApplicationDbContext context, IMapper mapper) : ICharacterProtectionService
{
    private readonly ApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<List<CharacterProtectionDto>>> GetByCharacterIdAsync(Guid characterId)
    {
        try
        {
            var characterProtections =
                await _context.CharacterProtections
                .Where(cp => cp.CharacterId == characterId)
                .Include(cp => cp.Protection)
                .ToListAsync();
            return new Result<List<CharacterProtectionDto>> { Value = _mapper.Map<List<CharacterProtectionDto>>(characterProtections) };
        }
        catch(Exception)
        {
            return new Result<List<CharacterProtectionDto>> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> CreateAsync(CharacterProtectionCreateDto characterProtectionCreate)
    {
        try
        {
            var characterProtection = _mapper.Map<CharacterProtection>(characterProtectionCreate);

            var existingCharacter = await _context.Characters.FindAsync(characterProtection.CharacterId);
            if (existingCharacter is null)
                return new Result<object> { IsError = true, Error = "Character not found" };

            var existingProtection = await _context.Protections.FindAsync(characterProtection.ProtectionId);
            if (existingProtection is null)
                return new Result<object> { IsError = true, Error = "Protection not found" };

            characterProtection.Character = existingCharacter;
            characterProtection.Protection = existingProtection;

            // Set the Encumbrance, Qualities, Slots based on the Protection entity
            characterProtection.Encumbrance = characterProtection.Protection.Encumbrance;
            characterProtection.Qualities = characterProtection.Protection.Qualities;
            characterProtection.Slots = characterProtection.Protection.Slots;

            _context.CharacterProtections.Add(characterProtection);
            await _context.SaveChangesAsync();
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> UpdateAsync(CharacterProtectionDto characterProtectionDto)
    {
        try
        {
            var existing = await _context.CharacterProtections.FirstOrDefaultAsync(cb => cb.Id == characterProtectionDto.Id);
            if (existing is null)
                return new Result<object> { IsError = true, Error = "CharacterProtection not found" };
            _mapper.Map(characterProtectionDto, existing);

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
            var existing = await _context.CharacterProtections.FirstOrDefaultAsync(cb => cb.Id == id);
            if (existing is null)
                return new Result<object> { IsError = true, Error = "CharacterProtection not found" };
            _context.CharacterProtections.Remove(existing);
            await _context.SaveChangesAsync();
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }
}
