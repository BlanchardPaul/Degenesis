namespace Degenesis.Shared.DTOs.Characters.CRUD;

public class CharacterAttributeDto
{
    public Guid CharacterId { get; set; }
    public Guid AttributeId { get; set; }
    public AttributeDto Attribute { get; set; } = new();
    public int Level { get; set; }
}