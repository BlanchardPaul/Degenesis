using Domain.Characters;

namespace Domain.Vehicles;
public class CharacterVehicle
{
    public Guid Id { get; set; }
    public Guid CharacterId { get; set; }
    public Character Character { get; set; } = new();
    public Guid VehicleId { get; set; }
    public Vehicle Vehicle { get; set; } = new();
    public int UsedSlots { get; set; } = 0;
    public int FleshLost { get; set; } = 0;
    public int TraumaLost { get; set; } = 0;
}