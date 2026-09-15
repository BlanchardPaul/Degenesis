using Domain.Weapons;

namespace Domain.Characters.Inventory;
public class CharacterWeapon
{
    public Guid Id { get; set; }
    public Guid CharacterId { get; set; }
    public Character Character { get; set; } = new();
    public Guid WeaponId { get; set; }
    public Weapon Weapon { get; set; } = new();
    public int BulletsInMagazine { get; set; }
    public int Encumbrance { get; set; }
    public int UsedSlots { get; set; }
    public int Slots { get; set; }
    public string Qualities { get; set; } = string.Empty;
}