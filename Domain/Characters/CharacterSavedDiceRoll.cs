namespace Domain.Characters;
public class CharacterSavedDiceRoll
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid CharacterId { get; set; }
    public Character Character { get; set; } = null!;
    public Guid AttributeId { get; set; }
    public CAttribute Attribute { get; set; } = null!;
    public Guid SkillId { get; set; }
    public Skill Skill { get; set; } = null!;
}
