namespace Degenesis.Shared.DTOs.Characters.Display;
public class CharacterAttributeDisplayDto
{
    public Guid AttributeId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Level { get; set; }
}