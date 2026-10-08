using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Characters.CRUD;
using Domain.Characters;
using Microsoft.EntityFrameworkCore;

namespace Business.Characters;

public interface IRankService
{
    Task<Result<List<RankDto>>> GetAllRanksAsync();
    Task<Result<RankDto>> GetRankByIdAsync(Guid id);
    Task<Result<object>> CreateRankAsync(RankCreateDto rankCreate);
    Task<Result<object>> UpdateRankAsync(RankDto rank);
    Task<Result<object>> DeleteRankAsync(Guid id);
}

public class RankService(ApplicationDbContext context, IMapper mapper) : IRankService
{
    private readonly ApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<List<RankDto>>> GetAllRanksAsync()
    {
        try
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
            .OrderBy(r => r.Name)
            .ToListAsync();

            return new Result<List<RankDto>> { Value = [.. ranks.Select(rank => _mapper.Map<RankDto>(rank))] };
        }
        catch (Exception)
        {
            return new Result<List<RankDto>> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<RankDto>> GetRankByIdAsync(Guid id)
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

            return new Result<RankDto> { Value = _mapper.Map<RankDto>(rank) };
        }
        catch (Exception)
        {
            return new Result<RankDto> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> CreateRankAsync(RankCreateDto rankCreate)
    {        
        try
        {
            var rank = _mapper.Map<Rank>(rankCreate);

            foreach (var prerequisite in rankCreate.Prerequisites)
            {
                var existingPerequisite = await _context.RankPrerequisites.FindAsync(prerequisite.Id);
                if (existingPerequisite is null)
                    return new Result<object> { IsError = true, Error = "Prerequisite not found" };

                rank.Prerequisites.Add(existingPerequisite);
            }

            var cult = await _context.Cults
                .FirstOrDefaultAsync(c => c.Id == rankCreate.CultId);
            if (cult is null)
                return new Result<object> { IsError = true, Error = "Cult not found" };
            rank.Cult = cult;

            _context.Ranks.Add(rank);
            await _context.SaveChangesAsync();
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> UpdateRankAsync(RankDto rankDto)
    {
        try
        {
            var existingRank = await _context.Ranks
                .Include(r => r.Prerequisites)
                .Include(r => r.Cult)
                .FirstOrDefaultAsync(r => r.Id == rankDto.Id);
            if (existingRank is null)
                return new Result<object> { IsError = true, Error = "Rank not found" };

            _mapper.Map(rankDto, existingRank);

            existingRank.Prerequisites.Clear();
            foreach (var prerequisite in rankDto.Prerequisites)
            {
                var existingPerequisite = await _context.RankPrerequisites.FindAsync(prerequisite.Id);
                if (existingPerequisite is null)
                    return new Result<object> { IsError = true, Error = "Prerequisite not found" };
                existingRank.Prerequisites.Add(existingPerequisite);
            }

            var cult = await _context.Cults
                .FirstOrDefaultAsync(c => c.Id == rankDto.CultId);
            if (cult is null)
                return new Result<object> { IsError = true, Error = "Cult not found" };
            existingRank.Cult = cult;

            await _context.SaveChangesAsync();
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> DeleteRankAsync(Guid id)
    {
        try
        {
            var rank = await _context.Ranks
                .Include(r => r.Prerequisites)
                .Include(r => r.Cult)
                .FirstOrDefaultAsync(r => r.Id == id);
            if (rank is null)
                return new Result<object> { IsError = true, Error = "Rank not found" };

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
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }
}
