using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs.Characters.CRUD.Inventory;
using Domain.Characters.Inventory;
using Microsoft.EntityFrameworkCore;

namespace Business.Characters.Inventory;
public interface ICharacterBurnService
{
    Task<CharacterBurnDto?> GetByIdAsync(Guid id);
    Task<List<CharacterBurnDto>> GetByCharacterIdAsync(Guid characterId);
    Task<CharacterBurnDto?> CreateAsync(CharacterBurnCreateDto characterBurn);
    Task<bool> UpdateAsync(CharacterBurnDto characterBurn);
    Task<bool> DeleteAsync(Guid id);
}

public class CharacterBurnService : ICharacterBurnService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CharacterBurnService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CharacterBurnDto?> GetByIdAsync(Guid id)
    {
        try
        {
            var characterBurn = await _context.CharacterBurns
                .Include(cb => cb.Burn)
                .FirstOrDefaultAsync(cb => cb.Id == id) ?? throw new Exception("CharacterBurn not found");
            return _mapper.Map<CharacterBurnDto>(characterBurn);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<List<CharacterBurnDto>> GetByCharacterIdAsync(Guid characterId)
    {
        var characterBurns =
            await _context.CharacterBurns
            .Where(cb => cb.CharacterId == characterId)
            .Include(cb => cb.Burn)
            .ToListAsync();
        return _mapper.Map<List<CharacterBurnDto>>(characterBurns);
    }

    public async Task<CharacterBurnDto?> CreateAsync(CharacterBurnCreateDto characterBurnCreate)
    {
        try
        {
            var characterBurn = _mapper.Map<CharacterBurn>(characterBurnCreate);
            characterBurn.Character = await _context.Characters.FindAsync(characterBurnCreate.CharacterId)
                ?? throw new Exception("Character not found");
            characterBurn.Burn = await _context.Burns.FindAsync(characterBurnCreate.BurnId)
                ?? throw new Exception("Burn not found");
            _context.CharacterBurns.Add(characterBurn);

            await _context.SaveChangesAsync();
            return _mapper.Map<CharacterBurnDto>(characterBurn);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<bool> UpdateAsync(CharacterBurnDto characterBurn)
    {
        try
        {
            var existing = await _context.CharacterBurns.FirstOrDefaultAsync(cb => cb.Id == characterBurn.Id) 
                ?? throw new Exception("CharacterBurn not found");
            _mapper.Map(characterBurn, existing);

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
            var characterBurn = await _context.CharacterBurns.FirstOrDefaultAsync(cb => cb.Id == id)
                ?? throw new Exception("CharacterBurn not found");

            _context.CharacterBurns.Remove(characterBurn);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
