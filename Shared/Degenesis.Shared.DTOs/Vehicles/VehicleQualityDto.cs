namespace Degenesis.Shared.DTOs.Vehicles;

public class VehicleQualityDto : VehicleQualityCreateDto
{
    public Guid Id { get; set; }
}

public class VehicleQualityCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}