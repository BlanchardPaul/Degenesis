namespace Degenesis.Shared.DTOs.Equipments;
public class EquipmentTypeDto : EquipmentTypeCreateDto
{
    public Guid Id { get; set; }
}

public class EquipmentTypeCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}