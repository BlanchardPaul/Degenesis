using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs.Characters;
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
                .Include(a => a.Background)
                .Include(a => a.Character)
            .FirstOrDefaultAsync(cb => cb.CharacterId == characterBackground.CharacterId && cb.BackgroundId == characterBackground.BackgroundId) ?? throw new Exception("CharacterBackground not found"); ;

            // We ignore the Attribute and character in the mapper, it will never be changed from here
            _mapper.Map(characterBackground, existingCharacterBackground);

            existingCharacterBackground.Background = await _context.Backgrounds.FirstOrDefaultAsync(a => a.Id == characterBackground.BackgroundId) ?? throw new Exception("Background not found");
            existingCharacterBackground.Character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == characterBackground.CharacterId) ?? throw new Exception("Character not found");
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}

