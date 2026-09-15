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
            .Include(p => p.Cults)
            .OrderBy(p => p.Name)
            .ToListAsync();
        return _mapper.Map<List<ProtectionDto>>(protections);
    }

    public async Task<ProtectionDto?> GetProtectionByIdAsync(Guid id)
    {
        try
        {
            var protection = await _context.Protections
                .Include(p => p.Cults)
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

            foreach (var cultDto in protectionCreate.Cults)
            {
                var cult = await _context.Cults
                    .FirstOrDefaultAsync(c => c.Id == cultDto.Id) ?? throw new Exception("Cult not found");
                protection.Cults.Add(cult);
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
                .Include(p => p.Cults)
                .FirstOrDefaultAsync(p => p.Id == protectionDto.Id)
                ?? throw new Exception("Protection not found");

            existingProtection.Cults.Clear();
            foreach (var cultDto in protectionDto.Cults)
            {
                var cult = await _context.Cults
                    .FirstOrDefaultAsync(c => c.Id == cultDto.Id) ?? throw new Exception("Cult not found");
                existingProtection.Cults.Add(cult);
            }


            _mapper.Map(protectionDto, existingProtection);

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
                .FirstOrDefaultAsync(p => p.Id == id)
                ?? throw new Exception("Protection not found");

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