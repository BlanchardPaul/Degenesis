namespace Degenesis.Shared.DTOs.Characters.CRUD;
public class CharacterSkillDto
{
    public Guid CharacterId { get; set; }
    public Guid SkillId { get; set; }
    public SkillDto Skill { get; set; } = new();
    public int Level { get; set; }
}