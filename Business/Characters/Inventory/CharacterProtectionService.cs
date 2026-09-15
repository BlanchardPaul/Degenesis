using DataAccessLayer;
using Degenesis.Shared.DTOs.Characters.CRUD.Inventory;
using AutoMapper;
using Domain.Characters.Inventory;
using Microsoft.EntityFrameworkCore;

public interface ICharacterProtectionService
{
    Task<List<CharacterProtectionDto>> GetByCharacterIdAsync(Guid characterId);
    Task<CharacterProtectionDto?> CreateAsync(CharacterProtectionCreateDto characterProtection);
    Task<bool> UpdateAsync(CharacterProtectionDto characterProtection);
    Task<bool> DeleteAsync(Guid id);
}

public class CharacterProtectionService : ICharacterProtectionService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CharacterProtectionService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<CharacterProtectionDto>> GetByCharacterIdAsync(Guid characterId)
    {
        var characterProtections =
            await _context.CharacterProtections
            .Where(cp => cp.CharacterId == characterId)
            .Include(cp => cp.Protection)
            .ToListAsync();
        return _mapper.Map<List<CharacterProtectionDto>>(characterProtections);
    }

    public async Task<CharacterProtectionDto?> CreateAsync(CharacterProtectionCreateDto characterProtectionCreate)
    {
        try
        {
            var characterProtection = _mapper.Map<CharacterProtection>(characterProtectionCreate);
            characterProtection.Character = await _context.Characters.FindAsync(characterProtectionCreate.CharacterId)
                ?? throw new Exception("Character not found");
            characterProtection.Protection = await _context.Protections.FindAsync(characterProtectionCreate.ProtectionId)
                ?? throw new Exception("Protection not found");

            // Set the Encumbrance, Qualities, Slots based on the Protection entity
            characterProtection.Encumbrance = characterProtection.Protection.Encumbrance;
            characterProtection.Qualities = characterProtection.Protection.Qualities;
            characterProtection.Slots = characterProtection.Protection.Slots;

            _context.CharacterProtections.Add(characterProtection);
            await _context.SaveChangesAsync();
            return _mapper.Map<CharacterProtectionDto>(characterProtection);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<bool> UpdateAsync(CharacterProtectionDto characterProtectionDto)
    {
        try
        {
            var existing = await _context.CharacterProtections.FirstOrDefaultAsync(cb => cb.Id == characterProtectionDto.Id)
                ?? throw new Exception("CharacterProtection not found");
            _mapper.Map(characterProtectionDto, existing);

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
            var existing = await _context.CharacterProtections.FirstOrDefaultAsync(cb => cb.Id == id)
                ?? throw new Exception("CharacterProtection not found");
            _context.CharacterProtections.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
