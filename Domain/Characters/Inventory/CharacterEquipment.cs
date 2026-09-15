using Domain.Equipments;

namespace Domain.Characters.Inventory;
public class CharacterEquipment
{
    public Guid Id { get; set; }
    public Guid CharacterId { get; set; }
    public Character Character { get; set; } = new();
    public Guid EquipmentId { get; set; }
    public Equipment Equipment { get; set; } = new();
}