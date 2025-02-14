namespace Domain.Characters;
public class CharacterBackground
{
    public Guid CharacterId { get; set; }
    public Character Character { get; set; } = new();
    public Guid BackgroundId { get; set; }
    public Background Background { get; set; } = new();
    public int Level { get; set; }
}
