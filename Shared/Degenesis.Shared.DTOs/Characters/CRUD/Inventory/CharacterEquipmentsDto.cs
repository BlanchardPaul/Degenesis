using Degenesis.Shared.DTOs.Equipments;

namespace Degenesis.Shared.DTOs.Characters.CRUD.Inventory;
public class CharacterEquipmentCreateDto
{
    public Guid Id { get; set; }
    public Guid CharacterId { get; set; }
    public Guid EquipmentId { get; set; }
}

public class CharacterEquipmentDto : CharacterEquipmentCreateDto
{
    public EquipmentDto Equipment { get; set; } = new();
}
