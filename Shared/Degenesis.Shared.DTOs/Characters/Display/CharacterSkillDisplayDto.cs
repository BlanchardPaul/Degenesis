namespace Degenesis.Shared.DTOs.Characters.Display;
public class CharacterSkillDisplayDto
{
    public Guid SkillId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Level { get; set; }
    public Guid AttributeId { get; set; }
}