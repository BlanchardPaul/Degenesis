using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Characters.CRUD;
using Degenesis.Shared.DTOs.Protections;
using Domain.Protections;
using Microsoft.EntityFrameworkCore;

namespace Business.Protections;
public interface IProtectionService
{
    Task<Result<List<ProtectionDto>>> GetAllProtectionsAsync();
    Task<Result<ProtectionDto>> GetProtectionByIdAsync(Guid id);
    Task<Result<object>> CreateProtectionAsync(ProtectionCreateDto protectionCreate);
    Task<Result<object>> UpdateProtectionAsync(ProtectionDto protection);
    Task<Result<object>> DeleteProtectionAsync(Guid id);
}

public class ProtectionService(ApplicationDbContext context, IMapper mapper) : IProtectionService
{
    private readonly ApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<List<ProtectionDto>>> GetAllProtectionsAsync()
    {
        try
        {
            var protections = await _context.Protections
                .Include(p => p.Cults)
                .OrderBy(p => p.Name)
                .ToListAsync();
            return new Result<List<ProtectionDto>> { Value = _mapper.Map<List<ProtectionDto>>(protections) };
        }
        catch(Exception)
        {
            return new Result<List<ProtectionDto>> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<ProtectionDto>> GetProtectionByIdAsync(Guid id)
    {
        try
        {
            var protection = await _context.Protections
                .Include(p => p.Cults)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (protection is null)
                return new Result<ProtectionDto> { IsError = true, Error = "Protection not found" };

            return new Result<ProtectionDto> { Value = _mapper.Map<ProtectionDto>(protection) };
        }
        catch (Exception)
        {
            return new Result<ProtectionDto> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> CreateProtectionAsync(ProtectionCreateDto protectionCreate)
    {
        try
        {
            var protection = _mapper.Map<Protection>(protectionCreate);

            foreach (var cultDto in protectionCreate.Cults)
            {
                var cult = await _context.Cults
                    .FirstOrDefaultAsync(c => c.Id == cultDto.Id);
                if (cult is null)
                    return new Result<object> { IsError = true, Error = "Cult not found" };
                protection.Cults.Add(cult);
            }

            _context.Protections.Add(protection);
            await _context.SaveChangesAsync();
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> UpdateProtectionAsync(ProtectionDto protectionDto)
    {
        try
        {
            var existingProtection = await _context.Protections
                .Include(p => p.Cults)
                .FirstOrDefaultAsync(p => p.Id == protectionDto.Id);
            if (existingProtection is null)
                return new Result<object> { IsError = true, Error = "Protection not found" };

            existingProtection.Cults.Clear();
            foreach (var cultDto in protectionDto.Cults)
            {
                var cult = await _context.Cults
                    .FirstOrDefaultAsync(c => c.Id == cultDto.Id);
                if (cult is null)
                    return new Result<object> { IsError = true, Error = "Cult not found" };
                existingProtection.Cults.Add(cult);
            }


            _mapper.Map(protectionDto, existingProtection);

            await _context.SaveChangesAsync();
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> DeleteProtectionAsync(Guid id)
    {
        try
        {
            var protection = await _context.Protections
                .FirstOrDefaultAsync(p => p.Id == id);
            if (protection is null)
                return new Result<object> { IsError = true, Error = "Protection not found" };

            _context.Protections.Remove(protection);
            await _context.SaveChangesAsync();
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }
}