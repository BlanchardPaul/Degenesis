using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs.Protections;
using Domain.Protections;
using Microsoft.EntityFrameworkCore;

namespace Business.Protections;
public interface IProtectionQualityService
{
    Task<IEnumerable<ProtectionQualityDto>> GetAllProtectionQualitiesAsync();
    Task<ProtectionQualityDto?> GetProtectionQualityByIdAsync(Guid id);
    Task<ProtectionQualityDto?> CreateProtectionQualityAsync(ProtectionQualityCreateDto protectionQualityCreate);
    Task<bool> UpdateProtectionQualityAsync(ProtectionQualityDto protectionQuality);
    Task<bool> DeleteProtectionQualityAsync(Guid id);
}

public class ProtectionQualityService : IProtectionQualityService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ProtectionQualityService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProtectionQualityDto>> GetAllProtectionQualitiesAsync()
    {
        var qualities = await _context.ProtectionQualities.ToListAsync();
        return _mapper.Map<IEnumerable<ProtectionQualityDto>>(qualities);
    }

    public async Task<ProtectionQualityDto?> GetProtectionQualityByIdAsync(Guid id)
    {
        var quality = await _context.ProtectionQualities.FindAsync(id);
        return quality is null ? null : _mapper.Map<ProtectionQualityDto>(quality);
    }

    public async Task<ProtectionQualityDto?> CreateProtectionQualityAsync(ProtectionQualityCreateDto protectionQualityCreate)
    {
        var quality = _mapper.Map<ProtectionQuality>(protectionQualityCreate);
        _context.ProtectionQualities.Add(quality);
        await _context.SaveChangesAsync();
        return _mapper.Map<ProtectionQualityDto>(quality);
    }

    public async Task<bool> UpdateProtectionQualityAsync(ProtectionQualityDto protectionQualityDto)
    {
        var existingQuality = await _context.ProtectionQualities.FindAsync(protectionQualityDto.Id);

        if (existingQuality is null)
            return false;

        _mapper.Map(protectionQualityDto, existingQuality);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteProtectionQualityAsync(Guid id)
    {
        var quality = await _context.ProtectionQualities.FindAsync(id);
        if (quality is null)
            return false;

        _context.ProtectionQualities.Remove(quality);
        await _context.SaveChangesAsync();
        return true;
    }
}