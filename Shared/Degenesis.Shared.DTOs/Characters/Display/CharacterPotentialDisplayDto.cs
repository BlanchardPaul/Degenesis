namespace Degenesis.Shared.DTOs.Characters.Display;
public class CharacterPotentialDisplayDto
{
    public Guid PotentialId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Level { get; set; }
}