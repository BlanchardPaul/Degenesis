namespace Degenesis.Shared.DTOs.Characters.CRUD;
public class CharacterPotentialDto
{
    public Guid CharacterId { get; set; }
    public Guid PotentialId { get; set; }
    public PotentialDto Potential { get; set; } = new();
    public int Level { get; set; }
}
