using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs.Characters;
using Domain.Characters;
using Microsoft.EntityFrameworkCore;
using System;

namespace Business.Characters;

public interface IBackgroundService
{
    Task<Background?> GetBackgroundByIdAsync(Guid id);
    Task<IEnumerable<Background>> GetAllBackgroundsAsync();
    Task<Background> CreateBackgroundAsync(BackgroundCreateDto background);
    Task<bool> UpdateBackgroundAsync(Background background);
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

    public async Task<Background?> GetBackgroundByIdAsync(Guid id)
    {
        return await _context.Backgrounds
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<IEnumerable<Background>> GetAllBackgroundsAsync()
    {
        return await _context.Backgrounds.ToListAsync();
    }

    public async Task<Background> CreateBackgroundAsync(BackgroundCreateDto backgroundCreate)
    {
        var background = _mapper.Map<Background>(backgroundCreate);
        _context.Backgrounds.Add(background);
        await _context.SaveChangesAsync();
        return background;
    }

    public async Task<bool> UpdateBackgroundAsync(Background background)
    {
        var existing = await _context.Backgrounds.FindAsync(background.Id);
        if (existing == null) return false;

        _mapper.Map(background, existing);

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteBackgroundAsync(Guid id)
    {
        var background = await _context.Backgrounds.FindAsync(id);
        if (background == null)
            return false;

        _context.Backgrounds.Remove(background);
        await _context.SaveChangesAsync();
        return true;
    }
}
