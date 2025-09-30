using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs.Characters;
using Domain.Characters;
using Microsoft.EntityFrameworkCore;

namespace Business.Characters;

public interface IConceptService
{
    Task<ConceptDto?> GetConceptByIdAsync(Guid id);
    Task<List<ConceptDto>> GetAllConceptsAsync();
    Task<ConceptDto?> CreateConceptAsync(ConceptCreateDto conceptCreate);
    Task<bool> UpdateConceptAsync(ConceptDto conceptDto);
    Task<bool> DeleteConceptAsync(Guid id);
}

public class ConceptService : IConceptService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ConceptService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ConceptDto?> GetConceptByIdAsync(Guid id)
    {
        try
        {
            var concept = await _context.Concepts
            .Include(c => c.BonusAttribute)
            .Include(c => c.BonusSkills)
            .FirstOrDefaultAsync(c => c.Id == id) ?? throw new Exception("Concept not found");
            return _mapper.Map<ConceptDto>(concept);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<List<ConceptDto>> GetAllConceptsAsync()
    {
        var concepts = await _context.Concepts
            .Include(c => c.BonusAttribute)
            .Include(c => c.BonusSkills)
            .ToListAsync();

        return _mapper.Map<List<ConceptDto>>(concepts);
    }

    public async Task<ConceptDto?> CreateConceptAsync(ConceptCreateDto conceptCreate)
    {
        try
        {
            var concept = _mapper.Map<Concept>(conceptCreate);

            var attribute = await _context.Attributes
                .FirstOrDefaultAsync(a => a.Id == conceptCreate.BonusAttributeId)
                ?? throw new Exception("Attribute not found");

            concept.BonusAttribute = attribute;

            foreach (var skillDto in conceptCreate.BonusSkills)
            {
                var existingSkill = await _context.Skills
                    .FirstOrDefaultAsync(s => s.Id == skillDto.Id)
                    ?? throw new Exception("Skill not found");

                concept.BonusSkills.Add(existingSkill);
            }

            _context.Concepts.Add(concept);
            await _context.SaveChangesAsync();
            return _mapper.Map<ConceptDto>(concept);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<bool> UpdateConceptAsync(ConceptDto conceptDto)
    {

        try
        {
            var existingConcept = await _context.Concepts
                .Include(c => c.BonusAttribute)
                .Include(c => c.BonusSkills)
                .FirstOrDefaultAsync(c => c.Id == conceptDto.Id) ?? throw new Exception("Concept not found");

            var attribute = await _context.Attributes
                .FirstOrDefaultAsync(a => a.Id == conceptDto.BonusAttributeId)
                ?? throw new Exception("Attribute not found");

            existingConcept.BonusAttribute = attribute;

            existingConcept.BonusSkills.Clear();
            foreach (var skillDto in conceptDto.BonusSkills)
            {
                var existingSkill = await _context.Skills
                    .FirstOrDefaultAsync(s => s.Id == skillDto.Id)
                    ?? throw new Exception("Skill not found");

                existingConcept.BonusSkills.Add(existingSkill);
            }

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> DeleteConceptAsync(Guid id)
    {
        try
        {
            var concept = await _context.Concepts
                .Include(c => c.BonusSkills)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (concept is null)
                return false;

            _context.Concepts.Remove(concept);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
