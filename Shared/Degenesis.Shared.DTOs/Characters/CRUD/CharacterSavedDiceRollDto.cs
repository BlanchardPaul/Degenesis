namespace Degenesis.Shared.DTOs.Characters.CRUD;


public class CharacterSavedDiceRollDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid CharacterId { get; set; }
    public Guid AttributeId { get; set; }
    public Guid SkillId { get; set; }
}

