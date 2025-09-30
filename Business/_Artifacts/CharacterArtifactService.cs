using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs._Artifacts;
using Domain._Artifacts;
using Microsoft.EntityFrameworkCore;

namespace Business._Artifacts;
public interface ICharacterArtifactService
{
    Task<CharacterArtifactDto?> GetByIdAsync(Guid id);
    Task<List<CharacterArtifactDto>> GetByCharacterIdAsync(Guid characterId);
    Task<CharacterArtifactDto?> CreateAsync(CharacterArtifactCreateDto characterArtifact);
    Task<bool> UpdateAsync(CharacterArtifactDto characterArtifact);
    Task<bool> DeleteAsync(Guid id);
}

public class CharacterArtifactService : ICharacterArtifactService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CharacterArtifactService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CharacterArtifactDto?> GetByIdAsync(Guid id)
    {
        try
        {
            var characterArtifact = await _context.CharacterArtifacts
                .Include(ca => ca.Artifact)
                .FirstOrDefaultAsync(ca => ca.Id == id) ?? throw new Exception("CharacterArtifact not found");
            return _mapper.Map<CharacterArtifactDto>(characterArtifact);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<List<CharacterArtifactDto>> GetByCharacterIdAsync(Guid characterId)
    {
        var characterArtifacts =
            await _context.CharacterArtifacts
            .Where(ca => ca.CharacterId == characterId)
            .Include(ca => ca.Artifact)
            .ToListAsync();
        return _mapper.Map<List<CharacterArtifactDto>>(characterArtifacts);
    }

    public async Task<CharacterArtifactDto?> CreateAsync(CharacterArtifactCreateDto characterArtifactCreate)
    {
        try
        {
            var characterArtifact = _mapper.Map<CharacterArtifact>(characterArtifactCreate);

            characterArtifact.Character = await _context.Characters.FindAsync(characterArtifactCreate.CharacterId) ?? throw new Exception("Character not found");
            characterArtifact.Artifact = await _context.Artifacts.FindAsync(characterArtifactCreate.ArtifactId) ?? throw new Exception("Artifact not found");
            _context.CharacterArtifacts.Add(characterArtifact);

            await _context.SaveChangesAsync();
            return _mapper.Map<CharacterArtifactDto>(characterArtifact);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<bool> UpdateAsync(CharacterArtifactDto characterArtifact)
    {
        try
        {
            var existing = await _context.CharacterArtifacts.FirstOrDefaultAsync(ca => ca.Id == characterArtifact.Id) ?? throw new Exception("CharacterArtifact not found");

            existing = _mapper.Map<CharacterArtifact>(characterArtifact);
            existing.Character = await _context.Characters.FindAsync(characterArtifact.CharacterId) ?? throw new Exception("Character not found");
            existing.Artifact = await _context.Artifacts.FindAsync(characterArtifact.ArtifactId) ?? throw new Exception("Artifact not found");

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
            var characterArtifact = await _context.CharacterArtifacts.FirstOrDefaultAsync(ca => ca.Id == id) ?? throw new Exception("CharacterArtifact not found");

            _context.CharacterArtifacts.Remove(characterArtifact);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}