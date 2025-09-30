using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs.Burns;
using Domain.Burns;
using Microsoft.EntityFrameworkCore;

namespace Business.Burns;
public interface IBurnService
{
    Task<List<BurnDto>> GetAllAsync();
    Task<BurnDto?> GetByIdAsync(Guid id);
    Task<BurnDto?> CreateAsync(BurnCreateDto burn);
    Task<bool> UpdateAsync(BurnDto burn);
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

    public async Task<List<BurnDto>> GetAllAsync()
    {
        var burns = await _context.Burns.ToListAsync();
        return _mapper.Map<List<BurnDto>>(burns);
    }

    public async Task<BurnDto?> GetByIdAsync(Guid id)
    {
        try
        {
            var burn = await _context.Burns.FirstOrDefaultAsync(b => b.Id == id) ?? throw new Exception("Burn not found");
            return _mapper.Map<BurnDto>(burn);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<BurnDto?> CreateAsync(BurnCreateDto burnCreate)
    {
        try
        {
            var burn = _mapper.Map<Burn>(burnCreate);
            _context.Burns.Add(burn);
            await _context.SaveChangesAsync();
            return _mapper.Map<BurnDto>(burn);
        }
        catch (Exception) {
            return null;
        }
    }

    public async Task<bool> UpdateAsync(BurnDto burn)
    {
        try
        {
            var existing = await _context.Burns.FirstOrDefaultAsync(b => b.Id == burn.Id) ?? throw new Exception("Burns not found");

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
            var existingBurn = await _context.Burns.FirstOrDefaultAsync(b => b.Id == id) ?? throw new Exception("Burns not found");

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
