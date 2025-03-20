using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs.Characters;
using Domain.Characters;
using Microsoft.EntityFrameworkCore;

namespace Business.Characters;

public interface IRankService
{
    Task<IEnumerable<RankDto>> GetAllRanksAsync();
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

    public async Task<IEnumerable<RankDto>> GetAllRanksAsync()
    {
        var ranks = await _context.Ranks
            .Include(r => r.Prerequisites)
                .ThenInclude(p => p.AttributeRequired)
            .Include(r => r.Prerequisites)
                .ThenInclude(p => p.SkillRequired)
            .Include(r => r.Cult)
            .ToListAsync();

        return _mapper.Map<IEnumerable<RankDto>>(ranks);
    }

    public async Task<RankDto?> GetRankByIdAsync(Guid id)
    {
        var rank = await _context.Ranks
            .Include(r => r.Prerequisites)
            .Include(r => r.Cult)
            .FirstOrDefaultAsync(r => r.Id == id);

        return rank is null ? null : _mapper.Map<RankDto>(rank);
    }

    public async Task<RankDto?> CreateRankAsync(RankCreateDto rankCreate)
    {
        var rank = _mapper.Map<Rank>(rankCreate);

        rank.Prerequisites = await _context.RankPrerequisites
            .Where(rp => rankCreate.Prerequisites.Select(p => p.Id).Contains(rp.Id))
            .ToListAsync();

        rank.Cult = await _context.Cults.FirstAsync(c => c.Id == rankCreate.CultId);

        _context.Ranks.Add(rank);
        await _context.SaveChangesAsync();
        return _mapper.Map<RankDto>(rank);
    }

    public async Task<bool> UpdateRankAsync(RankDto rankDto)
    {
        var existingRank = await _context.Ranks
            .Include(r => r.Prerequisites)
            .Include(r => r.Cult)
            .FirstOrDefaultAsync(r => r.Id == rankDto.Id);

        if (existingRank is null || rankDto.Cult is null)
            return false;

        _mapper.Map(rankDto, existingRank);

        existingRank.Prerequisites = await _context.RankPrerequisites
            .Where(rp => rankDto.Prerequisites.Select(p => p.Id).Contains(rp.Id))
            .ToListAsync();

        existingRank.Cult = await _context.Cults.FirstAsync(c => c.Id == rankDto.Cult.Id);

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteRankAsync(Guid id)
    {
        var rank = await _context.Ranks.FindAsync(id);
        if (rank == null)
            return false;

        _context.Ranks.Remove(rank);
        await _context.SaveChangesAsync();
        return true;
    }
}
