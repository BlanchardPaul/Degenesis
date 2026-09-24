using Domain.Burns;

namespace Domain.Characters.Inventory;
public class CharacterBurn
{
    public Guid Id { get; set; }
    public Guid CharacterId { get; set; }
    public Character Character { get; set; } = new();
    public Guid BurnId { get; set; }
    public Burn Burn { get; set; } = new();
    public int WeakQuantity { get; set; }
    public int PotentQuantity { get; set; }
}
