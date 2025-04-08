using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs.Characters;
using Domain.Characters;
using Microsoft.EntityFrameworkCore;

namespace Business.Characters;

public interface IConceptService
{
    Task<ConceptDto?> GetConceptByIdAsync(Guid id);
    Task<IEnumerable<ConceptDto>> GetAllConceptsAsync();
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
        return await _context.Concepts
            .Include(c => c.BonusAttribute)
            .Include(c => c.BonusSkills)
            .Where(c => c.Id == id)
            .Select(c => new ConceptDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                BonusAttribute = new AttributeDto
                {
                    Id = c.BonusAttribute.Id,
                    Name = c.BonusAttribute.Name,
                    Abbreviation = c.BonusAttribute.Abbreviation,
                    Description = c.BonusAttribute.Description
                },
                BonusSkills = c.BonusSkills.Select(s => new SkillDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    CAttribute = new AttributeDto
                    {
                        Id = s.CAttribute.Id,
                        Name = s.CAttribute.Name,
                        Abbreviation = s.CAttribute.Abbreviation,
                        Description = s.CAttribute.Description
                    }
                }).ToList()
            })
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<ConceptDto>> GetAllConceptsAsync()
    {
        return await _context.Concepts
            .Include(c => c.BonusAttribute)
            .Include(c => c.BonusSkills)
            .Select(c => new ConceptDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                BonusAttribute = new AttributeDto
                {
                    Id = c.BonusAttribute.Id,
                    Name = c.BonusAttribute.Name,
                    Abbreviation = c.BonusAttribute.Abbreviation,
                    Description = c.BonusAttribute.Description
                },
                BonusSkills = c.BonusSkills.Select(s => new SkillDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    CAttribute = new AttributeDto
                    {
                        Id = s.CAttribute.Id,
                        Name = s.CAttribute.Name,
                        Abbreviation = s.CAttribute.Abbreviation,
                        Description = s.CAttribute.Description
                    }
                }).ToList()
            })
            .ToListAsync();
    }

    public async Task<ConceptDto?> CreateConceptAsync(ConceptCreateDto conceptCreate)
    {
        try
        {
            var concept = _mapper.Map<Concept>(conceptCreate);

            // Gérer la relation avec BonusAttribute
            if (conceptCreate.BonusAttributeId != Guid.Empty)
            {
                var attribute = await _context.Attributes.FindAsync(conceptCreate.BonusAttributeId);
                if (attribute != null)
                {
                    concept.BonusAttribute = attribute;
                }
            }

            // Gérer la relation avec BonusSkills
            concept.BonusSkills = new List<Skill>();
            foreach (var skillDto in conceptCreate.BonusSkills)
            {
                var existingSkill = await _context.Skills.FindAsync(skillDto.Id);
                if (existingSkill != null)
                {
                    concept.BonusSkills.Add(existingSkill);
                }
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
                .FirstOrDefaultAsync(c => c.Id == conceptDto.Id);

            if (existingConcept is null)
                throw new Exception("Concept not found");

            _context.Entry(existingConcept).CurrentValues.SetValues(conceptDto);

            if (conceptDto.BonusAttribute != null)
            {
                var attribute = await _context.Attributes.FindAsync(conceptDto.BonusAttribute.Id);
                if (attribute != null)
                {
                    existingConcept.BonusAttribute = attribute;
                }
            }

            existingConcept.BonusSkills.Clear();
            foreach (var skillDto in conceptDto.BonusSkills)
            {
                var skill = await _context.Skills.FindAsync(skillDto.Id);
                if (skill != null)
                {
                    existingConcept.BonusSkills.Add(skill);
                }
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
                .Include(c => c.BonusSkills) // Inclure les relations pour les supprimer
                .FirstOrDefaultAsync(c => c.Id == id);

            if (concept is null)
                return false;

            // Supprimer les relations avec BonusSkills avant de supprimer le Concept
            concept.BonusSkills.Clear();
            await _context.SaveChangesAsync();

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
