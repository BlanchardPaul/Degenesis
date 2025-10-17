using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs.Characters.CRUD;
using Domain.Characters;
using Microsoft.EntityFrameworkCore;

namespace Business.Characters;
public interface ICharacterSkillService
{
    Task<bool> UpdateCharacterSkillAsync(CharacterSkillDto characterSkill);
}
public class CharacterSkillService : ICharacterSkillService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CharacterSkillService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<bool> UpdateCharacterSkillAsync(CharacterSkillDto characterSkill)
    {
        try
        {
            var existingCharacterSkill = await _context.CharacterSkills
                .Include(a => a.Skill)
                .Include(a => a.Character)
                .FirstOrDefaultAsync(cs => cs.CharacterId == characterSkill.CharacterId && cs.SkillId == characterSkill.SkillId)
                ?? throw new Exception("CharacterSkill not found");

            _mapper.Map(characterSkill, existingCharacterSkill);

            existingCharacterSkill.Skill = await _context.Skills.FirstOrDefaultAsync(a => a.Id == characterSkill.SkillId) ?? throw new Exception("Skill not found");
            existingCharacterSkill.Character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == characterSkill.CharacterId) ?? throw new Exception("Character not found");
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

}
