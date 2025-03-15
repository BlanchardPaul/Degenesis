namespace Degenesis.Shared.DTOs.Vehicles;
public class VehicleTypeDto : VehicleTypeCreateDto
{
    public Guid Id { get; set; }
}

public class VehicleTypeCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}