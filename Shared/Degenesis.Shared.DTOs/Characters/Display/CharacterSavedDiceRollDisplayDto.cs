namespace Degenesis.Shared.DTOs.Characters.Display;
public class CharacterSavedDiceRollDisplayDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid CharacterId { get; set; }
    public Guid AttributeId { get; set; }
    public string AttributeName { get; set; } = null!;
    public Guid SkillId { get; set; }
    public string SkillName { get; set; } = null!;
}
