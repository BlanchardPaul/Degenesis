namespace Degenesis.Shared.DTOs.Characters;
public class CharacterSkillDto
{
    public Guid CharacterId { get; set; }
    public Guid SkillId { get; set; }
    public int Level { get; set; }
}