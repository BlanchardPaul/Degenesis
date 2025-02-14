using Domain.NPCs;

namespace Domain.Weapons;
public class NPCWeapon
{
    public Guid Id { get; set; }
    public Guid NPCId { get; set; }
    public NPC NPC { get; set; } = new();
    public Guid WeaponId { get; set; }
    public Weapon Weapon { get; set; } = new();
    public int BulletsInMagazine { get; set; }
    public int UsedSlots { get; set; }
    public string SlotAttachments { get; set; } = string.Empty;
}