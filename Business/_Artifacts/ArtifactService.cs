using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs._Artifacts;
using Domain._Artifacts;
using Microsoft.EntityFrameworkCore;

namespace Business._Artifacts;
public interface IArtifactService
{
    Task<IEnumerable<Artifact>> GetAllAsync();
    Task<Artifact?> GetByIdAsync(Guid id);
    Task<Artifact?> CreateAsync(ArtifactCreateDto artifact);
    Task<bool> UpdateAsync(Artifact artifact);
    Task<bool> DeleteAsync(Guid id);
}

public class ArtifactService : IArtifactService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ArtifactService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<Artifact>> GetAllAsync()
    {
        return await _context.Artifacts.ToListAsync();
    }

    public async Task<Artifact?> GetByIdAsync(Guid id)
    {
        return await _context.Artifacts.FindAsync(id);
    }

    public async Task<Artifact?> CreateAsync(ArtifactCreateDto artifactCreate)
    {
        try
        {
            var artifact = _mapper.Map<Artifact>(artifactCreate);
            _context.Artifacts.Add(artifact);
            await _context.SaveChangesAsync();
            return artifact;
        }
        catch (Exception) { 
            return null;
        }
    }

    public async Task<bool> UpdateAsync(Artifact artifact)
    {
        try
        {
            var existing = await _context.Artifacts.FindAsync(artifact.Id);
            if (existing is null) return false;

            _mapper.Map(artifact, existing);

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
            var artifact = await _context.Artifacts.FindAsync(id);
            if (artifact is null) return false;

            _context.Artifacts.Remove(artifact);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}