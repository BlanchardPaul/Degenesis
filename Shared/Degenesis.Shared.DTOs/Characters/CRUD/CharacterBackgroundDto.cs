namespace Degenesis.Shared.DTOs.Characters.CRUD;
public class CharacterBackgroundDto
{
    public Guid CharacterId { get; set; }
    public Guid BackgroundId { get; set; }
    public BackgroundDto Background { get; set; } = new();
    public int Level { get; set; }
}