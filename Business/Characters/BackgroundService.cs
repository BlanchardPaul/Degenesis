using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Characters.CRUD;
using Domain.Characters;
using Microsoft.EntityFrameworkCore;

namespace Business.Characters;

public interface IBackgroundService
{
    Task<Result<List<BackgroundDto>>> GetAllBackgroundsAsync();
    Task<Result<BackgroundDto>> GetBackgroundByIdAsync(Guid id);
    Task<Result<object>> CreateBackgroundAsync(BackgroundCreateDto background);
    Task<Result<object>> UpdateBackgroundAsync(BackgroundDto background);
    Task<Result<object>> DeleteBackgroundAsync(Guid id);
}
public class BackgroundService(ApplicationDbContext context, IMapper mapper) : IBackgroundService
{
    private readonly ApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<List<BackgroundDto>>> GetAllBackgroundsAsync()
    {
        try
        {
            var backgrounds = await _context.Backgrounds.OrderBy(b => b.Name).ToListAsync();
            return new Result<List<BackgroundDto>> { Value = _mapper.Map<List<BackgroundDto>>(backgrounds) };
        }
        catch (Exception)
        {
            return new Result<List<BackgroundDto>> { IsError = true, Error = "A server error occurred" };
        }

    }

    public async Task<Result<BackgroundDto>> GetBackgroundByIdAsync(Guid id)
    {
        try
        {
            var background = await _context.Backgrounds.FirstOrDefaultAsync(b => b.Id == id);
            if (background is null)
                return new Result<BackgroundDto> { IsError = true, Error = "Background not found" };

            return new Result<BackgroundDto> { Value = _mapper.Map<BackgroundDto>(background) };
        }
        catch (Exception)
        {
            return new Result<BackgroundDto> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> CreateBackgroundAsync(BackgroundCreateDto backgroundCreate)
    {
        try
        {
            var background = _mapper.Map<Background>(backgroundCreate);
            _context.Backgrounds.Add(background);
            await _context.SaveChangesAsync();
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> UpdateBackgroundAsync(BackgroundDto background)
    {
        try
        {
            var existing = await _context.Backgrounds
                .FirstOrDefaultAsync(b => b.Id == background.Id);

            if (existing is null)
                return new Result<object> { IsError = true, Error = "Background not found" };

            _mapper.Map(background, existing);

            await _context.SaveChangesAsync();
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> DeleteBackgroundAsync(Guid id)
    {
        try
        {
            var background = await _context.Backgrounds
                .FirstOrDefaultAsync(b => b.Id == id);
            if (background is null)
                return new Result<object> { IsError = true, Error = "Background not found" };

            _context.Backgrounds.Remove(background);
            await _context.SaveChangesAsync();
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }
}
