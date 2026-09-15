using Degenesis.Shared.DTOs.Weapons;

namespace Degenesis.Shared.DTOs.Characters.CRUD.Inventory;
public class CharacterWeaponCreateDto
{
    public Guid CharacterId { get; set; }
    public Guid WeaponId { get; set; }
}

public class CharacterWeaponDto : CharacterWeaponCreateDto
{
    public Guid Id { get; set; }
    public int BulletsInMagazine { get; set; }
    public int Encumbrance { get; set; }
    public int UsedSlots { get; set; }
    public int Slots { get; set; }
    public string Qualities { get; set; } = string.Empty;
    public WeaponDto Weapon { get; set; } = new();
}