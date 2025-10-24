using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs.Characters.CRUD;
using Microsoft.EntityFrameworkCore;

namespace Business.Characters;
public interface ICharacterBackgroundService
{
    Task<bool> UpdateCharacterBackgroundAsync(CharacterBackgroundDto characterBackground);
}
public class CharacterBackgroundService : ICharacterBackgroundService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CharacterBackgroundService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<bool> UpdateCharacterBackgroundAsync(CharacterBackgroundDto characterBackground)
    {
        try
        {
            var existingCharacterBackground = await _context.CharacterBackgrounds
                .FirstOrDefaultAsync(cb => cb.CharacterId == characterBackground.CharacterId && cb.BackgroundId == characterBackground.BackgroundId) 
                ?? throw new Exception("CharacterBackground not found"); ;

            existingCharacterBackground.Level = characterBackground.Level;
             
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}

