using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs._Artifacts;
using Degenesis.Shared.DTOs.Characters;
using Domain._Artifacts;
using Domain.Characters;
using Microsoft.EntityFrameworkCore;
using System;

namespace Business._Artifacts;
public interface IArtifactService
{
    Task<List<ArtifactDto>> GetAllAsync();
    Task<ArtifactDto?> GetByIdAsync(Guid id);
    Task<ArtifactDto?> CreateAsync(ArtifactCreateDto artifact);
    Task<bool> UpdateAsync(ArtifactDto artifact);
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

    public async Task<List<ArtifactDto>> GetAllAsync()
    {
        var artifacts = await _context.Artifacts.ToListAsync();
        return _mapper.Map<List<ArtifactDto>>(artifacts);
    }

    public async Task<ArtifactDto?> GetByIdAsync(Guid id)
    {
        try
        {
            var artifact = await _context.Artifacts.FirstOrDefaultAsync(a => a.Id == id) ?? throw new Exception("Background not found");
            return _mapper.Map<ArtifactDto>(artifact);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<ArtifactDto?> CreateAsync(ArtifactCreateDto artifactCreate)
    {
        try
        {
            var artifact = _mapper.Map<Artifact>(artifactCreate);
            _context.Artifacts.Add(artifact);
            await _context.SaveChangesAsync();
            return _mapper.Map<ArtifactDto>(artifact);
        }
        catch (Exception) { 
            return null;
        }
    }

    public async Task<bool> UpdateAsync(ArtifactDto artifact)
    {
        try
        {
            var existing = await _context.Artifacts.FirstOrDefaultAsync(a => a.Id == artifact.Id) ?? throw new Exception("Background not found");
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
            var artifact = await _context.Artifacts.FirstOrDefaultAsync(a => a.Id == id) ?? throw new Exception("Background not found");
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