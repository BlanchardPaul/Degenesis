using Domain.Characters;

namespace Domain.Vehicles;
public class Vehicle
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int MaxSpeed { get; set; } = 0;
    public int Acceleration { get; set; } = 0;
    public int Brake { get; set; } = 0;
    public int Armor { get; set; } = 0;
    public int BodyFlesh { get; set; } = 1;
    public int StructureTrauma { get; set; } = 1;
    public int TechLevel { get; set; } = 0;
    public int Slots { get; set; } = 0;
    public string Value { get; set; } = string.Empty;

    // Necessary ressource to acquire said vehicle
    public string Resources { get; set; } = string.Empty;

    public Guid? CultId { get; set; }
    public Cult? Cult { get; set; } = new();
    public Guid VehicleTypeId { get; set; }
    public VehicleType VehicleType { get; set; } = new();
    public List<VehicleQuality> VehicleQualities { get; set; } = [];
}