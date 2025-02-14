using Domain.NPCs;

namespace Domain.Equipments;
public class NPCEquipment
{
    public Guid Id { get; set; }
    public Guid NPCId { get; set; }
    public NPC NPC { get; set; } = new();
    public Guid EquipmentId { get; set; }
    public Equipment Equipment { get; set; } = new();
    public int UsedSlots { get; set; }
}