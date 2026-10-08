using DataAccessLayer;
using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Characters.CRUD;
using Domain.Characters;
using Microsoft.EntityFrameworkCore;

namespace Business.Characters;
public interface ICharacterPotentialService
{
    Task<Result<object>> CreateCharacterPotentialAsync(CharacterGuidValueEditDto potentialToCreate);
    Task<Result<object>> UpdateCharacterPotentialAsync(CharacterPotentialDto characterPotential);
    Task<Result<object>> DeleteCharacterPotentialAsync(Guid characterId, Guid characterPotentialId);
}

public class CharacterPotentialService(ApplicationDbContext context) : ICharacterPotentialService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Result<object>> CreateCharacterPotentialAsync(CharacterGuidValueEditDto potentialToCreate)
    {
        try
        {
            var existingCharacter = await _context.Characters
                .FirstOrDefaultAsync(cp => cp.Id == potentialToCreate.Id);
            if (existingCharacter is null)
                return new Result<object> { IsError = true, Error = "Character not found" };

            var existingPotential = await _context.Potentials
                .FirstOrDefaultAsync(cp => cp.Id == potentialToCreate.Value);
            if (existingPotential is null)
                return new Result<object> { IsError = true, Error = "Potential not found" };

            CharacterPotential toCreate = new()
            {
                CharacterId = potentialToCreate.Id,
                Character = existingCharacter,
                PotentialId = potentialToCreate.Value,
                Potential = existingPotential,
                Level = 0
            };

            await _context.CharacterPotentials.AddAsync(toCreate);
            await _context.SaveChangesAsync();
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> UpdateCharacterPotentialAsync(CharacterPotentialDto characterPotential)
    {
        try
        {
            var existingCharacterPotential = await _context.CharacterPotentials
                .FirstOrDefaultAsync(cp => cp.CharacterId == characterPotential.CharacterId && cp.PotentialId == characterPotential.PotentialId) 
                ?? throw new Exception("CharacterPotential not found");
            if(existingCharacterPotential is null)
                return new Result<object> { IsError = true, Error = "CharacterPotential not found" };

            existingCharacterPotential.Level = characterPotential.Level;

            await _context.SaveChangesAsync();
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> DeleteCharacterPotentialAsync(Guid characterId, Guid characterPotentialId)
    {
        try
        {
            var existingCharacterPotential = await _context.CharacterPotentials
                .FirstOrDefaultAsync(cp => cp.CharacterId == characterId && cp.PotentialId == characterPotentialId);
            if (existingCharacterPotential is null)
                return new Result<object> { IsError = true, Error = "CharacterPotential not found" };

            _context.Remove(existingCharacterPotential);

            await _context.SaveChangesAsync();
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }
}
