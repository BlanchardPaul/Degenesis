using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Characters.CRUD;
using Domain.Characters;
using Microsoft.EntityFrameworkCore;

namespace Business.Characters;

public interface IConceptService
{
    Task<Result<ConceptDto>> GetConceptByIdAsync(Guid id);
    Task<Result<List<ConceptDto>>> GetAllConceptsAsync();
    Task<Result<object>> CreateConceptAsync(ConceptCreateDto conceptCreate);
    Task<Result<object>> UpdateConceptAsync(ConceptDto conceptDto);
    Task<Result<object>> DeleteConceptAsync(Guid id);
}

public class ConceptService(ApplicationDbContext context, IMapper mapper) : IConceptService
{
    private readonly ApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<ConceptDto>> GetConceptByIdAsync(Guid id)
    {
        try
        {
            var concept = await _context.Concepts
                .Include(c => c.BonusAttribute)
                .Include(c => c.BonusSkills)
                .OrderBy(c => c.Name)
                .FirstOrDefaultAsync(c => c.Id == id);
            if(concept is null)
                return new Result<ConceptDto> { IsError = true, Error = "Concept not found" };

            return new Result<ConceptDto> { Value = _mapper.Map<ConceptDto>(concept) };
        }
        catch (Exception)
        {
            return new Result<ConceptDto> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<List<ConceptDto>>> GetAllConceptsAsync()
    {
        try
        {
            var concepts = await _context.Concepts
                .Include(c => c.BonusAttribute)
                .Include(c => c.BonusSkills)
                .ToListAsync();
            return new Result<List<ConceptDto>> { Value = _mapper.Map<List<ConceptDto>>(concepts) };
        }
        catch (Exception)
        {
            return new Result<List<ConceptDto>> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> CreateConceptAsync(ConceptCreateDto conceptCreate)
    {
        try
        {
            var concept = _mapper.Map<Concept>(conceptCreate);

            var attribute = await _context.Attributes
                .FirstOrDefaultAsync(a => a.Id == conceptCreate.BonusAttributeId);
            if (attribute is null)
                return new Result<object> { IsError = true, Error = "Attribute not found" };

            concept.BonusAttribute = attribute;

            foreach (var skillDto in conceptCreate.BonusSkills)
            {
                var existingSkill = await _context.Skills
                    .FirstOrDefaultAsync(s => s.Id == skillDto.Id);
                if (existingSkill is null)
                    return new Result<object> { IsError = true, Error = "Skill not found" };

                concept.BonusSkills.Add(existingSkill);
            }

            _context.Concepts.Add(concept);
            await _context.SaveChangesAsync();
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> UpdateConceptAsync(ConceptDto conceptDto)
    {

        try
        {
            var existingConcept = await _context.Concepts
                .Include(c => c.BonusAttribute)
                .Include(c => c.BonusSkills)
                .FirstOrDefaultAsync(c => c.Id == conceptDto.Id);
            if (existingConcept is null)
                return new Result<object> { IsError = true, Error = "Concept not found" };

            var attribute = await _context.Attributes
                .FirstOrDefaultAsync(a => a.Id == conceptDto.BonusAttributeId);
            if (attribute is null)
                return new Result<object> { IsError = true, Error = "Attribute not found" };

            existingConcept.BonusAttribute = attribute;

            existingConcept.BonusSkills.Clear();
            foreach (var skillDto in conceptDto.BonusSkills)
            {
                var existingSkill = await _context.Skills
                    .FirstOrDefaultAsync(s => s.Id == skillDto.Id);
                if (existingSkill is null)
                    return new Result<object> { IsError = true, Error = "Skill not found" };

                existingConcept.BonusSkills.Add(existingSkill);
            }

            await _context.SaveChangesAsync();
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> DeleteConceptAsync(Guid id)
    {
        try
        {
            var concept = await _context.Concepts
                .Include(c => c.BonusSkills)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (concept is null)
                return new Result<object> { IsError = true, Error = "Concept not found" };

            var conceptSkills = _context.Set<ConceptSkill>().Where(cs => cs.ConceptId == id);
            _context.Set<ConceptSkill>().RemoveRange(conceptSkills);

            _context.Concepts.Remove(concept);
            await _context.SaveChangesAsync();
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }
}
