using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs.Protections;
using Domain.Protections;
using Microsoft.EntityFrameworkCore;

namespace Business.Protections;
public interface IProtectionService
{
    Task<List<ProtectionDto>> GetAllProtectionsAsync();
    Task<ProtectionDto?> GetProtectionByIdAsync(Guid id);
    Task<ProtectionDto?> CreateProtectionAsync(ProtectionCreateDto protectionCreate);
    Task<bool> UpdateProtectionAsync(ProtectionDto protection);
    Task<bool> DeleteProtectionAsync(Guid id);
}

public class ProtectionService : IProtectionService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ProtectionService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<ProtectionDto>> GetAllProtectionsAsync()
    {
        var protections = await _context.Protections
            .Include(p => p.Qualities)
            .ToListAsync();
        return _mapper.Map<List<ProtectionDto>>(protections);
    }

    public async Task<ProtectionDto?> GetProtectionByIdAsync(Guid id)
    {
        try
        {
            var protection = await _context.Protections
                .Include(p => p.Qualities)
                .FirstOrDefaultAsync(p => p.Id == id) ?? throw new Exception("Protection not found");

            return _mapper.Map<ProtectionDto>(protection);
        }
        catch (Exception)
        {
            return null;
        }

    }

    public async Task<ProtectionDto?> CreateProtectionAsync(ProtectionCreateDto protectionCreate)
    {
        try
        {
            var protection = _mapper.Map<Protection>(protectionCreate);

            foreach(var quality in protectionCreate.Qualities)
            {
                var existingQuality = await _context.ProtectionQualities.FindAsync(quality.Id)
                    ?? throw new Exception("ProtectionQuality not found");
                protection.Qualities.Add(existingQuality);
            }

            _context.Protections.Add(protection);
            await _context.SaveChangesAsync();
            return _mapper.Map<ProtectionDto>(protection);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<bool> UpdateProtectionAsync(ProtectionDto protectionDto)
    {
        try
        {
            var existingProtection = await _context.Protections
                .Include(p => p.Qualities)
                .FirstOrDefaultAsync(p => p.Id == protectionDto.Id)
                ?? throw new Exception("Protection not found");

            _mapper.Map(protectionDto, existingProtection);

            existingProtection.Qualities.Clear();
            foreach (var quality in protectionDto.Qualities)
            {
                var existingQuality = await _context.ProtectionQualities.FindAsync(quality.Id)
                    ?? throw new Exception("ProtectionQuality not found");
                existingProtection.Qualities.Add(existingQuality);
            }

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> DeleteProtectionAsync(Guid id)
    {
        try
        {
            var protection = await _context.Protections
                .Include(p => p.Qualities)
                .FirstOrDefaultAsync(p => p.Id == id)
                ?? throw new Exception("ProtectionQuality not found");

            _context.Protections.Remove(protection);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}