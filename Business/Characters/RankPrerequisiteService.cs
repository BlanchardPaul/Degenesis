using DataAccessLayer;
using Domain.Characters;
using Microsoft.EntityFrameworkCore;

namespace Business.Characters;
public interface IRankPrerequisiteService
{
    Task<List<RankPrerequisite>> GetAllRankPrerequisitesAsync();
    Task<RankPrerequisite?> GetRankPrerequisiteByIdAsync(Guid id);
    Task<RankPrerequisite> CreateRankPrerequisiteAsync(RankPrerequisite rankPrerequisite);
    Task<RankPrerequisite?> UpdateRankPrerequisiteAsync(Guid id, RankPrerequisite rankPrerequisite);
    Task<bool> DeleteRankPrerequisiteAsync(Guid id);
}
public class RankPrerequisiteService : IRankPrerequisiteService
{
    private readonly ApplicationDbContext _context;

    public RankPrerequisiteService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<RankPrerequisite>> GetAllRankPrerequisitesAsync()
    {
        return await _context.RankPrerequisites
            .Include(rp => rp.AttributeRequired)
            .Include(rp => rp.SkillRequired)
            .ToListAsync();
    }

    public async Task<RankPrerequisite?> GetRankPrerequisiteByIdAsync(Guid id)
    {
        return await _context.RankPrerequisites
            .Include(rp => rp.AttributeRequired)
            .Include(rp => rp.SkillRequired)
            .FirstOrDefaultAsync(rp => rp.Id == id);
    }

    public async Task<RankPrerequisite> CreateRankPrerequisiteAsync(RankPrerequisite rankPrerequisite)
    {
        _context.RankPrerequisites.Add(rankPrerequisite);
        await _context.SaveChangesAsync();
        return rankPrerequisite;
    }

    public async Task<RankPrerequisite?> UpdateRankPrerequisiteAsync(Guid id, RankPrerequisite rankPrerequisite)
    {
        var existingRankPrerequisite = await _context.RankPrerequisites.FindAsync(id);
        if (existingRankPrerequisite == null)
        {
            return null;
        }

        existingRankPrerequisite = rankPrerequisite;

        await _context.SaveChangesAsync();
        return existingRankPrerequisite;
    }

    public async Task<bool> DeleteRankPrerequisiteAsync(Guid id)
    {
        var existingRankPrerequisite = await _context.RankPrerequisites.FindAsync(id);
        if (existingRankPrerequisite == null)
        {
            return false;
        }

        _context.RankPrerequisites.Remove(existingRankPrerequisite);
        await _context.SaveChangesAsync();
        return true;
    }
}
