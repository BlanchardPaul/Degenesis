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

    // Whatever you want, food/fuel cumsuption, ...)
    public string Resources { get; set; } = string.Empty;

    public Guid VehicleTypeId { get; set; }
    public VehicleType VehicleType { get; set; } = new();
}