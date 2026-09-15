using Domain.Vehicles;

namespace Domain.Characters.Inventory;
public class CharacterVehicle
{
    public Guid Id { get; set; }
    public Guid CharacterId { get; set; }
    public Character Character { get; set; } = new();
    public Guid VehicleId { get; set; }
    public Vehicle Vehicle { get; set; } = new();
    public int UsedSlots { get; set; } = 0;
    public int Slots { get; set; } = 0;
    public string Qualities { get; set; } = string.Empty;
    public int BodyFlesh { get; set; } = 1;
    public int StructureTrauma { get; set; } = 1;
    public int BodyFleshLost { get; set; } = 0;
    public int StructureTraumaLost { get; set; } = 0;
}