namespace Domain.Equipments;
public class Equipment
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Capacity { get; set; } = string.Empty;
    public string Effect { get; set; } = string.Empty;
    public int Encumbrance { get; set; } = 0;
    public int TechLevel { get; set; } = 1;
    public int Slots { get; set; } = 0;
    public int Value { get; set; } = 0;
    public string Resources { get; set; } = string.Empty;
    public string EnergyStorage { get; set; } = string.Empty;
    public Guid EquipmentTypeId { get; set; }
    public EquipmentType EquipmentType { get; set; } = new();
}