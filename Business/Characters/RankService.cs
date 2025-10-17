using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs.Characters.CRUD;
using Domain.Characters;
using Domain.Protections;
using Microsoft.EntityFrameworkCore;

namespace Business.Characters;

public interface IRankService
{
    Task<List<RankDto>> GetAllRanksAsync();
    Task<RankDto?> GetRankByIdAsync(Guid id);
    Task<RankDto?> CreateRankAsync(RankCreateDto rankCreate);
    Task<bool> UpdateRankAsync(RankDto rank);
    Task<bool> DeleteRankAsync(Guid id);
}

public class RankService : IRankService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public RankService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<RankDto>> GetAllRanksAsync()
    {
        var ranks = await _context.Ranks
            .OrderBy(r => r.Name)
            .Include(r => r.Prerequisites)
                .ThenInclude(p => p.AttributeRequired)
            .Include(r => r.Prerequisites)
                .ThenInclude(p => p.SkillRequired)
            .Include(r => r.Prerequisites)
                .ThenInclude(p => p.BackgroundRequired)
            .Include(r => r.Cult)
            .Include(r => r.ParentRank)
            .ToListAsync();
        
        return ranks.Select(rank => _mapper.Map<RankDto>(rank)).ToList();
    }

    public async Task<RankDto?> GetRankByIdAsync(Guid id)
    {
        try
        {
            var rank = await _context.Ranks
                .Include(r => r.Prerequisites)
                    .ThenInclude(p => p.AttributeRequired)
                .Include(r => r.Prerequisites)
                    .ThenInclude(p => p.SkillRequired)
                .Include(r => r.Prerequisites)
                    .ThenInclude(p => p.BackgroundRequired)
                .Include(r => r.Cult)
                .Include(r => r.ParentRank)
                .FirstOrDefaultAsync(r => r.Id == id) 
                ?? throw new Exception("Rank not found");

            return _mapper.Map<RankDto>(rank);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<RankDto?> CreateRankAsync(RankCreateDto rankCreate)
    {        
        try
        {
            var rank = _mapper.Map<Rank>(rankCreate);

            foreach (var prerequisite in rankCreate.Prerequisites)
            {
                var existingPerequisite = await _context.RankPrerequisites.FindAsync(prerequisite.Id)
                    ?? throw new Exception("Perequisite not found");
                rank.Prerequisites.Add(existingPerequisite);
            }

            rank.Cult = await _context.Cults
                .FirstOrDefaultAsync(c => c.Id == rankCreate.CultId) 
                ?? throw new Exception("Cult not found");

            _context.Ranks.Add(rank);
            await _context.SaveChangesAsync();
            return _mapper.Map<RankDto>(rank);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<bool> UpdateRankAsync(RankDto rankDto)
    {
        try
        {
            var existingRank = await _context.Ranks
                .Include(r => r.Prerequisites)
                .Include(r => r.Cult)
                .FirstOrDefaultAsync(r => r.Id == rankDto.Id)
                 ?? throw new Exception("Rank not found");

            _mapper.Map(rankDto, existingRank);

            existingRank.Prerequisites.Clear();
            foreach (var prerequisite in rankDto.Prerequisites)
            {
                var existingPerequisite = await _context.RankPrerequisites.FindAsync(prerequisite.Id)
                    ?? throw new Exception("Perequisite not found");
                existingRank.Prerequisites.Add(existingPerequisite);
            }

            existingRank.Cult = await _context.Cults
                .FirstOrDefaultAsync(c => c.Id == rankDto.CultId)
                ?? throw new Exception("Cult not found");

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> DeleteRankAsync(Guid id)
    {
        try
        {
            var rank = await _context.Ranks
                .Include(r => r.Prerequisites)
                .Include(r => r.Cult)
                .FirstOrDefaultAsync(r => r.Id == id)
                 ?? throw new Exception("Rank not found");

            // We also have to remove the dependant ranks
            // Find dependent ranks
            var dependentRanks = await _context.Ranks
                .Where(r => r.ParentRankId == id)
                .ToListAsync();

            // Recursively delete dependents
            foreach (var dependent in dependentRanks)
            {
                await DeleteRankAsync(dependent.Id);
            }

            _context.Ranks.Remove(rank);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
