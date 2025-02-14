using Domain.Characters;

namespace Domain.Weapons;
public class CharacterWeapon
{
    public Guid Id { get; set; }
    public Guid CharacterId { get; set; }
    public Character Character { get; set; } = new();
    public Guid WeaponId { get; set; }
    public Weapon Weapon { get; set; } = new();
    public int BulletsInMagazine { get; set; }
    public int UsedSlots { get; set; }
    public string SlotAttachments { get; set; } = string.Empty;
}