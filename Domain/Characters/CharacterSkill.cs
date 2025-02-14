namespace Domain.Characters;

public class CharacterSkill
{
    public Guid CharacterId { get; set; }
    public Character Character { get; set; } = new();
    public Guid SkillId { get; set; }
    public Skill Skill { get; set; } = new();
    public int Level { get; set; }
}
