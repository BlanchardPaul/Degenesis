namespace Domain.Characters;

public class CharacterPotential
{
    public Guid CharacterId { get; set; }
    public Character Character { get; set; } = new();
    public Guid PotentialId { get; set; }
    public Potential Potential { get; set; } = new();
    public int Level { get; set; }
}