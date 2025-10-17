using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs.Characters.CRUD;
using Domain.Characters;
using Microsoft.EntityFrameworkCore;

namespace Business.Characters;
public interface ICharacterPotentialService
{
    Task<bool> UpdateCharacterPotentialAsync(CharacterPotentialDto characterPotential);
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

    public async Task<bool> UpdateCharacterPotentialAsync(CharacterPotentialDto characterPotential)
    {
        try
        {
            var existingCharacterPotential = await _context.CharacterPotentials
                .FirstOrDefaultAsync(cp => cp.CharacterId == characterPotential.CharacterId && cp.PotentialId == characterPotential.PotentialId) 
                ?? throw new Exception("CharacterPotential not found");
            _mapper.Map(characterPotential, existingCharacterPotential);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
