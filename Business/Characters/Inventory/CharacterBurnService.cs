using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Characters.CRUD.Inventory;
using Domain.Characters.Inventory;
using Microsoft.EntityFrameworkCore;

namespace Business.Characters.Inventory;
public interface ICharacterBurnService
{
    Task<Result<CharacterBurnDto>> GetByIdAsync(Guid id);
    Task<Result<List<CharacterBurnDto>>> GetByCharacterIdAsync(Guid characterId);
    Task<Result<object>> CreateAsync(CharacterBurnCreateDto characterBurn);
    Task<Result<object>> UpdateAsync(CharacterBurnDto characterBurn);
    Task<Result<object>> DeleteAsync(Guid id);
}

public class CharacterBurnService(ApplicationDbContext context, IMapper mapper) : ICharacterBurnService
{
    private readonly ApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<CharacterBurnDto>> GetByIdAsync(Guid id)
    {
        try
        {
            var characterBurn = await _context.CharacterBurns
                .Include(cb => cb.Burn)
                .FirstOrDefaultAsync(cb => cb.Id == id);
            if(characterBurn is null)
                return new Result<CharacterBurnDto> { IsError = true, Error = "CharacterBurn not found" };

            return new Result<CharacterBurnDto> { Value = _mapper.Map<CharacterBurnDto>(characterBurn) };
        }
        catch (Exception)
        {
            return new Result<CharacterBurnDto> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<List<CharacterBurnDto>>> GetByCharacterIdAsync(Guid characterId)
    {
        try
        {
            var characterBurns =
                await _context.CharacterBurns
                .Where(cb => cb.CharacterId == characterId)
                .Include(cb => cb.Burn)
                .ToListAsync();
            return new Result<List<CharacterBurnDto>> { Value = _mapper.Map<List<CharacterBurnDto>>(characterBurns) };
        }
        catch (Exception)
        {
            return new Result<List<CharacterBurnDto>> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> CreateAsync(CharacterBurnCreateDto characterBurnCreate)
    {
        try
        {
            var characterBurn = _mapper.Map<CharacterBurn>(characterBurnCreate);

            var existingCharacter = await _context.Characters.FindAsync(characterBurnCreate.CharacterId);
            if (existingCharacter is null)
                return new Result<object> { IsError = true, Error = "Character not found" };

            var existingBurn = await _context.Burns.FindAsync(characterBurnCreate.BurnId);
            if (existingBurn is null)
                return new Result<object> { IsError = true, Error = "Burn not found" };

            characterBurn.Character = existingCharacter;
            characterBurn.Burn = existingBurn;

            _context.CharacterBurns.Add(characterBurn);
            await _context.SaveChangesAsync();
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> UpdateAsync(CharacterBurnDto characterBurn)
    {
        try
        {
            var existing = await _context.CharacterBurns.FirstOrDefaultAsync(cb => cb.Id == characterBurn.Id);
            if (existing is null)
                return new Result<object> { IsError = true, Error = "CharacterBurn not found" };

            _mapper.Map(characterBurn, existing);
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
            var characterBurn = await _context.CharacterBurns.FirstOrDefaultAsync(cb => cb.Id == id);
            if (characterBurn is null)
                return new Result<object> { IsError = true, Error = "CharacterBurn not found" };

            _context.CharacterBurns.Remove(characterBurn);
            await _context.SaveChangesAsync();
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }
}
