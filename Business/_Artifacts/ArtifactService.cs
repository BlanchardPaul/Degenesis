using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs._Artifacts;
using Domain._Artifacts;
using Microsoft.EntityFrameworkCore;

namespace Business._Artifacts;
public interface IArtifactService
{
    Task<Result<List<ArtifactDto>>> GetAllAsync();
    Task<Result<ArtifactDto>> GetByIdAsync(Guid id);
    Task<Result<object>> CreateAsync(ArtifactCreateDto artifact);
    Task<Result<object>> UpdateAsync(ArtifactDto artifact);
    Task<Result<object>> DeleteAsync(Guid id);
}

public class ArtifactService(ApplicationDbContext context, IMapper mapper) : IArtifactService
{
    private readonly ApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<List<ArtifactDto>>> GetAllAsync()
    {
        try
        {
            var artifacts = await _context.Artifacts.OrderBy(a => a.Name).ToListAsync();
            return new Result<List<ArtifactDto>> { Value = _mapper.Map<List<ArtifactDto>>(artifacts) };
        }
        catch (Exception)
        {
            return new Result<List<ArtifactDto>> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<ArtifactDto>> GetByIdAsync(Guid id)
    {
        try
        {
            var artifact = await _context.Artifacts.FirstOrDefaultAsync(a => a.Id == id);
            if (artifact is null)
                return new Result<ArtifactDto> { IsError = true, Error = "Artifact not found" };

            return new Result<ArtifactDto> { Value = _mapper.Map<ArtifactDto>(artifact) };
        }
        catch (Exception)
        {
            return new Result<ArtifactDto> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> CreateAsync(ArtifactCreateDto artifactCreate)
    {
        try
        {
            var artifact = _mapper.Map<Artifact>(artifactCreate);
            _context.Artifacts.Add(artifact);
            await _context.SaveChangesAsync();
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> UpdateAsync(ArtifactDto artifact)
    {
        try
        {
            var existingArtifact = await _context.Artifacts.FirstOrDefaultAsync(a => a.Id == artifact.Id);
            if (existingArtifact is null) 
                return new Result<object> { IsError = true, Error = "Artifact not found" };

            _mapper.Map(artifact, existingArtifact);

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
            var existingArtifact = await _context.Artifacts.FirstOrDefaultAsync(a => a.Id == id);
            if (existingArtifact is null) 
                return new Result<object> { IsError = true, Error = "Artifact not found" };

            _context.Artifacts.Remove(existingArtifact);
            await _context.SaveChangesAsync();
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }
}