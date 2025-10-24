using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs.Characters.CRUD;
using Domain.Characters;
using Microsoft.EntityFrameworkCore;

namespace Business.Characters;
public interface ICharacterPotentialService
{
    Task<bool> CreateCharacterPotentialAsync(CharacterGuidValueEditDto potentialToCreate);
    Task<bool> UpdateCharacterPotentialAsync(CharacterPotentialDto characterPotential);
    Task<bool> DeleteCharacterPotentialAsync(Guid characterId, Guid characterPotentialId);
}

public class CharacterPotentialService : ICharacterPotentialService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CharacterPotentialService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<bool> CreateCharacterPotentialAsync(CharacterGuidValueEditDto potentialToCreate)
    {
        try
        {
            var existingCharacter = await _context.Characters
                .FirstOrDefaultAsync(cp => cp.Id == potentialToCreate.Id)
                ?? throw new Exception("Character not found");

            var existingPotential = await _context.Potentials
                .FirstOrDefaultAsync(cp => cp.Id == potentialToCreate.Value)
                ?? throw new Exception("Potential not found");

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
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> UpdateCharacterPotentialAsync(CharacterPotentialDto characterPotential)
    {
        try
        {
            var existingCharacterPotential = await _context.CharacterPotentials
                .FirstOrDefaultAsync(cp => cp.CharacterId == characterPotential.CharacterId && cp.PotentialId == characterPotential.PotentialId) 
                ?? throw new Exception("CharacterPotential not found");

            existingCharacterPotential.Level = characterPotential.Level;

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> DeleteCharacterPotentialAsync(Guid characterId, Guid characterPotentialId)
    {
        try
        {
            var existingCharacterPotential = await _context.CharacterPotentials
                .FirstOrDefaultAsync(cp => cp.CharacterId == characterId && cp.PotentialId == characterPotentialId)
                ?? throw new Exception("CharacterPotential not found");

            _context.Remove(existingCharacterPotential);

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
