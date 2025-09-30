using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs.Protections;
using Domain.Protections;
using Microsoft.EntityFrameworkCore;

namespace Business.Protections;
public interface IProtectionQualityService
{
    Task<List<ProtectionQualityDto>> GetAllProtectionQualitiesAsync();
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

    public async Task<List<ProtectionQualityDto>> GetAllProtectionQualitiesAsync()
    {
        var qualities = await _context.ProtectionQualities.ToListAsync();
        return _mapper.Map<List<ProtectionQualityDto>>(qualities);
    }

    public async Task<ProtectionQualityDto?> GetProtectionQualityByIdAsync(Guid id)
    {
        try
        {
            var quality = await _context.ProtectionQualities.FindAsync(id) 
                ?? throw new Exception("ProtectionQuality not found");
            return _mapper.Map<ProtectionQualityDto>(quality);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<ProtectionQualityDto?> CreateProtectionQualityAsync(ProtectionQualityCreateDto protectionQualityCreate)
    {
        try
        {
            var quality = _mapper.Map<ProtectionQuality>(protectionQualityCreate);
            _context.ProtectionQualities.Add(quality);
            await _context.SaveChangesAsync();
            return _mapper.Map<ProtectionQualityDto>(quality);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<bool> UpdateProtectionQualityAsync(ProtectionQualityDto protectionQualityDto)
    {
        try
        {
            var existingQuality = await _context.ProtectionQualities.FindAsync(protectionQualityDto.Id)
                ?? throw new Exception("ProtectionQuality not found");

            _mapper.Map(protectionQualityDto, existingQuality);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> DeleteProtectionQualityAsync(Guid id)
    {
        try
        {
            var quality = await _context.ProtectionQualities.FindAsync(id)
                ?? throw new Exception("ProtectionQuality not found");

            _context.ProtectionQualities.Remove(quality);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}