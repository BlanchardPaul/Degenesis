namespace Degenesis.Shared.DTOs.Protections;
public class ProtectionQualityDto : ProtectionQualityCreateDto
{
    public Guid Id { get; set; }
}

public class ProtectionQualityCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}