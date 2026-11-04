using Business.Hubs;
using DataAccessLayer;
using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Characters.CRUD;
using Domain.Characters;
using Microsoft.EntityFrameworkCore;

namespace Business.Characters;
public interface ICharacterPotentialService
{
    Task<Result<object>> CreateCharacterPotentialAsync(IdGuidEditDto potentialToCreate);
    Task<Result<object>> UpdateCharacterPotentialAsync(CharacterPotentialDto characterPotential);
    Task<Result<object>> DeleteCharacterPotentialAsync(Guid characterId, Guid characterPotentialId);
}

public class CharacterPotentialService(ApplicationDbContext context, IRoomBroadcastService broadcast) : ICharacterPotentialService
{
    private readonly ApplicationDbContext _context = context;
    private readonly IRoomBroadcastService _broadcast = broadcast;

    public async Task<Result<object>> CreateCharacterPotentialAsync(IdGuidEditDto potentialToCreate)
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
            await _broadcast.NotifyCharacterUpdatedAsync(existingCharacter.Id);
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
                .FirstOrDefaultAsync(cp => cp.CharacterId == characterPotential.CharacterId && cp.PotentialId == characterPotential.PotentialId);
            if(existingCharacterPotential is null)
                return new Result<object> { IsError = true, Error = "CharacterPotential not found" };

            var character = await _context.Characters
                .FirstOrDefaultAsync(c => c.Id == existingCharacterPotential.CharacterId);
            if (character is null)
                return new Result<object> { IsError = true, Error = "Character not found" };

            int oldValue = existingCharacterPotential.Level;
            int newValue = characterPotential.Level;

            int totalPotentialLevels = await _context.CharacterPotentials
                .Where(cp => cp.CharacterId == character.Id)
                .SumAsync(cp => cp.Level);
            int cost = CharacterXpCalculator.PotentialCost(totalPotentialLevels, oldValue, newValue);

            if (newValue > oldValue && character.Experience < cost)
                return new Result<object> { IsError = true, Error = $"Not enough XP! Needed: {cost}, Available: {character.Experience}" };

            character.Experience += newValue > oldValue ? -cost : cost;

            existingCharacterPotential.Level = newValue;

            await _context.SaveChangesAsync();
            await _broadcast.NotifyCharacterUpdatedAsync(existingCharacterPotential.CharacterId);
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
            await _broadcast.NotifyCharacterUpdatedAsync(existingCharacterPotential.CharacterId);
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }
}
