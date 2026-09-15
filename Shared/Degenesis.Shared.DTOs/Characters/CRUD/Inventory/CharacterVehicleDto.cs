using Degenesis.Shared.DTOs.Vehicles;

namespace Degenesis.Shared.DTOs.Characters.CRUD.Inventory;
public class CharacterVehicleCreateDto
{
    public Guid CharacterId { get; set; }
    public Guid VehicleId { get; set; }
}

public class CharacterVehicleDto : CharacterVehicleCreateDto
{
    public Guid Id { get; set; }
    public int UsedSlots { get; set; } = 0;
    public int Slots { get; set; } = 0;
    public string Qualities { get; set; } = string.Empty;
    public int BodyFlesh { get; set; } = 1;
    public int StructureTrauma { get; set; } = 1;
    public int BodyFleshLost { get; set; } = 0;
    public int StructureTraumaLost { get; set; } = 0;
    public VehicleDto Vehicle { get; set; } = new();
}
