using Domain.Characters;

namespace Domain.Burns;
public class CharacterBurn
{
    public Guid CharacterId { get; set; }
    public Character Character { get; set; } = new();
    public Guid BurnId { get; set; }
    public Burn Burn { get; set; } = new();
    public int Quantity { get; set; } = 1;
}