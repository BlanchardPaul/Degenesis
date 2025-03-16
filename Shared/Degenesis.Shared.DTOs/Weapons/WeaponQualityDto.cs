namespace Degenesis.Shared.DTOs.Weapons;
public class WeaponQualityDto : WeaponQualityCreateDto
{
    public Guid Id { get; set; }
}

public class WeaponQualityCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}