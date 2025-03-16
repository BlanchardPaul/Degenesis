namespace Degenesis.Shared.DTOs.Weapons;
public class WeaponTypeDto : WeaponTypeCreateDto
{
    public Guid Id { get; set; }
}

public class WeaponTypeCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}