using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Characters.CRUD.Inventory;
using Domain.Characters.Inventory;
using Microsoft.EntityFrameworkCore;

namespace Business.Characters.Inventory;

public interface ICharacterArtifactService
{
    Task<Result<CharacterArtifactDto>> GetByIdAsync(Guid id);
    Task<Result<List<CharacterArtifactDto>>> GetByCharacterIdAsync(Guid characterId);
    Task<Result<object>> CreateAsync(CharacterArtifactCreateDto characterArtifact);
    Task<Result<object>> UpdateAsync(CharacterArtifactDto characterArtifact);
    Task<Result<object>> DeleteAsync(Guid id);
}

public class CharacterArtifactService(ApplicationDbContext context, IMapper mapper) : ICharacterArtifactService
{
    private readonly ApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<CharacterArtifactDto>> GetByIdAsync(Guid id)
    {
        try
        {
            var characterArtifact = await _context.CharacterArtifacts
                .Include(ca => ca.Artifact)
                .FirstOrDefaultAsync(ca => ca.Id == id);
            if (characterArtifact is null)
                return new Result<CharacterArtifactDto> { IsError = true, Error = "CharacterArtifact not found" };

            return new Result<CharacterArtifactDto> { Value = _mapper.Map<CharacterArtifactDto>(characterArtifact) };
        }
        catch (Exception)
        {
            return new Result<CharacterArtifactDto> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<List<CharacterArtifactDto>>> GetByCharacterIdAsync(Guid characterId)
    {
        try
        {
            var characterArtifacts =
                await _context.CharacterArtifacts
                .Where(ca => ca.CharacterId == characterId)
                .Include(ca => ca.Artifact)
            .ToListAsync();
            return new Result<List<CharacterArtifactDto>> { Value = _mapper.Map<List<CharacterArtifactDto>>(characterArtifacts) };
        }
        catch (Exception)
        {
            return new Result<List<CharacterArtifactDto>> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> CreateAsync(CharacterArtifactCreateDto characterArtifactCreate)
    {
        try
        {
            var characterArtifact = _mapper.Map<CharacterArtifact>(characterArtifactCreate);

            var existingCharacter = await _context.Characters.FindAsync(characterArtifactCreate.CharacterId);
            if (existingCharacter is null)
                return new Result<object> { IsError = true, Error = "Character not found" };

            var existingArtifact = await _context.Artifacts.FindAsync(characterArtifactCreate.ArtifactId);
            if (existingArtifact is null)
                return new Result<object> { IsError = true, Error = "Artifact not found" };

            characterArtifact.Character = existingCharacter;
            characterArtifact.Artifact = existingArtifact;

            _context.CharacterArtifacts.Add(characterArtifact);
            await _context.SaveChangesAsync();
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> UpdateAsync(CharacterArtifactDto characterArtifact)
    {
        try
        {
            var existing = await _context.CharacterArtifacts.FirstOrDefaultAsync(ca => ca.Id == characterArtifact.Id);
            if (existing is null)
                return new Result<object> { IsError = true, Error = "CharacterArtifact not found" };

            _mapper.Map(characterArtifact, existing);
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
            var characterArtifact = await _context.CharacterArtifacts.FirstOrDefaultAsync(ca => ca.Id == id);
            if (characterArtifact is null)
                return new Result<object> { IsError = true, Error = "CharacterArtifact not found" };

            _context.CharacterArtifacts.Remove(characterArtifact);
            await _context.SaveChangesAsync();
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }
}