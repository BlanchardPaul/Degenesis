using Domain.Characters;

namespace Domain.Equipments;
public class CharacterEquipment
{
    public Guid Id { get; set; }
    public Guid CharacterId { get; set; }
    public Character Character { get; set; } = new();
    public Guid EquipmentId { get; set; }
    public Equipment Equipment { get; set; } = new();
    public int UsedSlots { get; set; }
}