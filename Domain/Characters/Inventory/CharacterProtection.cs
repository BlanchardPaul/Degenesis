using Domain.Protections;

namespace Domain.Characters.Inventory;
public class CharacterProtection
{
    public Guid Id { get; set; }
    public Guid CharacterId { get; set; }
    public Character Character { get; set; } = new();
    public Guid ProtectionId { get; set; }
    public Protection Protection { get; set; } = new();

    // Qualities, Encumbrance, Slots are stored again here because they can be modified by the character and are
    // not necessarily the same as the Protection's qualities and encumbrance.
    public string Qualities { get; set; } = string.Empty;
    public int Encumbrance { get; set; } = 0;
    // The Used... will be used to display Used.../Max...
    public int UsedSlots { get; set; } = 0;
    public int Slots { get; set; } = 0;
}