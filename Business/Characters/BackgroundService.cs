using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs.Characters;
using Domain.Characters;
using Microsoft.EntityFrameworkCore;

namespace Business.Characters;

public interface IBackgroundService
{
    Task<BackgroundDto?> GetBackgroundByIdAsync(Guid id);
    Task<List<BackgroundDto>> GetAllBackgroundsAsync();
    Task<BackgroundDto?> CreateBackgroundAsync(BackgroundCreateDto background);
    Task<bool> UpdateBackgroundAsync(BackgroundDto background);
    Task<bool> DeleteBackgroundAsync(Guid id);
}
public class BackgroundService : IBackgroundService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public BackgroundService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BackgroundDto?> GetBackgroundByIdAsync(Guid id)
    {
        try
        {
            var background = await _context.Backgrounds.FirstOrDefaultAsync(b => b.Id == id) ?? throw new Exception("Background not found");
            return _mapper.Map<BackgroundDto>(background);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<List<BackgroundDto>> GetAllBackgroundsAsync()
    {
        var backgrounds = await _context.Backgrounds.ToListAsync();
        return _mapper.Map<List<BackgroundDto>>(backgrounds);
    }

    public async Task<BackgroundDto?> CreateBackgroundAsync(BackgroundCreateDto backgroundCreate)
    {
        try
        {
            var background = _mapper.Map<Background>(backgroundCreate);
            _context.Backgrounds.Add(background);
            await _context.SaveChangesAsync();
            return _mapper.Map<BackgroundDto>(background); ;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<bool> UpdateBackgroundAsync(BackgroundDto background)
    {
        try
        {
            var existing = await _context.Backgrounds
                .FirstOrDefaultAsync(b => b.Id == background.Id) 
                ?? throw new Exception("Background not found");

            _mapper.Map(background, existing);

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> DeleteBackgroundAsync(Guid id)
    {
        try
        {
            var background = await _context.Backgrounds
                .FirstOrDefaultAsync(b => b.Id == id) 
                ?? throw new Exception("Background not found");
            if (background is null)
                return false;

            _context.Backgrounds.Remove(background);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
