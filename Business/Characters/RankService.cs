using DataAccessLayer;
using Domain.Characters;
using Microsoft.EntityFrameworkCore;

namespace Business.Characters;
public interface IRankService
{
    Task<List<Rank>> GetAllRanksAsync();
    Task<Rank?> GetRankByIdAsync(Guid id);
    Task<Rank> CreateRankAsync(Rank rank);
    Task<Rank?> UpdateRankAsync(Guid id, Rank rank);
    Task<bool> DeleteRankAsync(Guid id);
}

public class RankService : IRankService
{
    private readonly ApplicationDbContext _context;

    public RankService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Rank>> GetAllRanksAsync()
    {
        return await _context.Ranks
            .Include(r => r.Prerequisites)
            .ToListAsync();
    }

    public async Task<Rank?> GetRankByIdAsync(Guid id)
    {
        return await _context.Ranks
            .Include(r => r.Prerequisites)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<Rank> CreateRankAsync(Rank rank)
    {
        _context.Ranks.Add(rank);
        await _context.SaveChangesAsync();
        return rank;
    }

    public async Task<Rank?> UpdateRankAsync(Guid id, Rank rank)
    {
        var existingRank = await _context.Ranks.FindAsync(id);
        if (existingRank == null)
        {
            return null;
        }

        existingRank = rank;

        await _context.SaveChangesAsync();
        return existingRank;
    }

    public async Task<bool> DeleteRankAsync(Guid id)
    {
        var existingRank = await _context.Ranks.FindAsync(id);
        if (existingRank == null)
        {
            return false;
        }

        _context.Ranks.Remove(existingRank);
        await _context.SaveChangesAsync();
        return true;
    }
}
