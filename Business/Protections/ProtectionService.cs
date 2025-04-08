using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs.Protections;
using Domain.Protections;
using Microsoft.EntityFrameworkCore;

namespace Business.Protections;
public interface IProtectionService
{
    Task<IEnumerable<ProtectionDto>> GetAllProtectionsAsync();
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

    public async Task<IEnumerable<ProtectionDto>> GetAllProtectionsAsync()
    {
        var protections = await _context.Protections
            .Include(p => p.Qualities)
            .ToListAsync();
        return _mapper.Map<IEnumerable<ProtectionDto>>(protections);
    }

    public async Task<ProtectionDto?> GetProtectionByIdAsync(Guid id)
    {
        var protection = await _context.Protections
            .Include(p => p.Qualities)
            .FirstOrDefaultAsync(p => p.Id == id);

        return protection is null ? null : _mapper.Map<ProtectionDto>(protection);
    }

    public async Task<ProtectionDto?> CreateProtectionAsync(ProtectionCreateDto protectionCreate)
    {
        try
        {
            var protection = _mapper.Map<Protection>(protectionCreate);
            protection.Qualities = await _context.ProtectionQualities
                .Where(q => protectionCreate.QualityIds.Contains(q.Id))
                .ToListAsync();

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
                .FirstOrDefaultAsync(p => p.Id == protectionDto.Id);

            if (existingProtection is null)
                return false;

            _mapper.Map(protectionDto, existingProtection);
            existingProtection.Qualities = await _context.ProtectionQualities
                .Where(q => protectionDto.Qualities.Select(qd => qd.Id).Contains(q.Id))
                .ToListAsync();

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
            var protection = await _context.Protections.FindAsync(id);
            if (protection is null)
                return false;

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