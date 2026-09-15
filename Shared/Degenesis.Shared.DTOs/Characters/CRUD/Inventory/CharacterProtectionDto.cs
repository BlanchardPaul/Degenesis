using Degenesis.Shared.DTOs.Protections;

namespace Degenesis.Shared.DTOs.Characters.CRUD.Inventory;

public class CharacterProtectionCreateDto
{
    public Guid Id { get; set; }
    public Guid CharacterId { get; set; }
    public Guid ProtectionId { get; set; }
}

public class CharacterProtectionDto : CharacterProtectionCreateDto
{
    public int UsedSlots { get; set; } = 0;
    public int Slots { get; set; } = 0;
    public string Qualities { get; set; } = string.Empty;
    public int Encumbrance { get; set; } = 0;
    public ProtectionDto Protection { get; set; } = new();
}