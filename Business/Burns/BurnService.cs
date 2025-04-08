using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs.Burns;
using Domain._Artifacts;
using Domain.Burns;
using Microsoft.EntityFrameworkCore;

namespace Business.Burns;
public interface IBurnService
{
    Task<List<Burn>> GetAllAsync();
    Task<Burn?> GetByIdAsync(Guid id);
    Task<Burn?> CreateAsync(BurnCreateDto burn);
    Task<bool> UpdateAsync(Burn burn);
    Task<bool> DeleteAsync(Guid id);
}
public class BurnService : IBurnService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public BurnService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<Burn>> GetAllAsync()
    {
        return await _context.Burns.ToListAsync();
    }

    public async Task<Burn?> GetByIdAsync(Guid id)
    {
        return await _context.Burns
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<Burn?> CreateAsync(BurnCreateDto burnCreate)
    {
        try
        {
            var burn = _mapper.Map<Burn>(burnCreate);
            _context.Burns.Add(burn);
            await _context.SaveChangesAsync();
            return burn;
        }
        catch (Exception) {
            return null;
        }
    }

    public async Task<bool> UpdateAsync(Burn burn)
    {
        try
        {
            var existing = await _context.Burns.FindAsync(burn.Id);
            if (existing is null) return false;

            _mapper.Map(burn, existing);

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception) { 
            return false;
        }
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        try
        {
            var existingBurn = await _context.Burns.FindAsync(id);
            if (existingBurn is null)
            {
                return false;
            }

            _context.Burns.Remove(existingBurn);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
