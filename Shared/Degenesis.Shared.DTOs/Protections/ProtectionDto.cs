namespace Degenesis.Shared.DTOs.Protections;
public class ProtectionDto : ProtectionCreateDto
{
    public Guid Id { get; set; }   
}

public class ProtectionCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int? Armor { get; set; }
    public List<ProtectionQualityDto> Qualities { get; set; } = [];
    public int Stockage { get; set; } = 0;
    public int Slots { get; set; } = 0;
    public int Connectors { get; set; } = 0;
    public string Consuption { get; set; } = string.Empty;
    public string Defense { get; set; } = string.Empty;
    public string Attack { get; set; } = string.Empty;
    public int Encumbrance { get; set; } = 0;
    public int TechLevel { get; set; } = 1;
    public int Value { get; set; } = 0;
    public string Resources { get; set; } = string.Empty;
}
