using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Burns;
using Domain.Burns;
using Microsoft.EntityFrameworkCore;

namespace Business.Burns;
public interface IBurnService
{
    Task<Result<List<BurnDto>>> GetAllAsync();
    Task<Result<BurnDto>> GetByIdAsync(Guid id);
    Task<Result<object>> CreateAsync(BurnCreateDto burn);
    Task<Result<object>> UpdateAsync(BurnDto burn);
    Task<Result<object>> DeleteAsync(Guid id);
}
public class BurnService(ApplicationDbContext context, IMapper mapper) : IBurnService
{
    private readonly ApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<List<BurnDto>>> GetAllAsync()
    {
        try
        {
            var burns = await _context.Burns.OrderBy(b => b.Name).ToListAsync();
            return new Result<List<BurnDto>>{ Value = _mapper.Map<List<BurnDto>>(burns) };
        }
        catch (Exception)
        {
            return new Result<List<BurnDto>> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<BurnDto>> GetByIdAsync(Guid id)
    {
        try
        {
            var burn = await _context.Burns.FirstOrDefaultAsync(b => b.Id == id);
            if (burn is null)
                return new Result<BurnDto> { IsError = true, Error = "Burn not found" };
            return new Result<BurnDto>{Value = _mapper.Map<BurnDto>(burn)};
        }
        catch (Exception)
        {
            return new Result<BurnDto> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> CreateAsync(BurnCreateDto burnCreate)
    {
        try
        {
            var burn = _mapper.Map<Burn>(burnCreate);
            _context.Burns.Add(burn);
            await _context.SaveChangesAsync();
            return new Result<object> {Value = null};
        }
        catch (Exception) {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> UpdateAsync(BurnDto burn)
    {
        try
        {
            var existingBurn = await _context.Burns.FirstOrDefaultAsync(b => b.Id == burn.Id);
            if (existingBurn is null)
                return new Result<object> { IsError = true, Error = "Burn not found" };

            _mapper.Map(burn, existingBurn);

            await _context.SaveChangesAsync();
            return new Result<object>{Value = true};
        }
        catch (Exception) { 
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> DeleteAsync(Guid id)
    {
        try
        {
            var existingBurn = await _context.Burns.FirstOrDefaultAsync(b => b.Id == id);
            if (existingBurn is null)
                return new Result<object> { IsError = true, Error = "Burn not found" };

            _context.Burns.Remove(existingBurn);
            await _context.SaveChangesAsync();
            return new Result<object>{Value = null};
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }
}
