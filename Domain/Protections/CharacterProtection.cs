using Domain.Characters;

namespace Domain.Protections;
public class CharacterProtection
{
    public Guid Id { get; set; }
    public Guid CharacterId { get; set; }
    public Character Character { get; set; } = new();
    public Guid ProtectionId { get; set; }
    public Protection Protection { get; set; } = new();
    // The Used... will be used to display Used.../Max...
    public int UsedConnectors { get; set; } = 0;
    public int UsedSlots { get; set; } = 0;
}