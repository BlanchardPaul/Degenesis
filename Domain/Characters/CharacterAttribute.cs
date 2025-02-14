namespace Domain.Characters;
public class CharacterAttribute
{
    public Guid CharacterId { get; set; }
    public Character Character { get; set; } = new();
    public Guid AttributeId { get; set; }
    public CAttribute Attribute { get; set; } = new();
    public int Level { get; set; }
}
