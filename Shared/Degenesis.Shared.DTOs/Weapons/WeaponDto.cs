using Degenesis.Shared.DTOs.Characters.CRUD;

namespace Degenesis.Shared.DTOs.Weapons;
public class WeaponDto : WeaponCreateDto
{
    public Guid Id { get; set; }
    public WeaponTypeDto WeaponType { get; set; } = new();
    public AttributeDto? Attribute { get; set; }
}

public class WeaponCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Caliber { get; set; } = string.Empty;
    public string Handling { get; set; } = string.Empty;
    public string Distance { get; set; } = string.Empty;
    public int? Damage { get; set; }
    public Guid? AttributeId { get; set; }
    public float? CharacterAttributeModifier { get; set; }
    public int Magazine { get; set; } = 0;
    public int Encumbrance { get; set; } = 0;
    public int TechLevel { get; set; } = 1;
    public int Slots { get; set; } = 0;
    public int Value { get; set; } = 0;
    public string Resources { get; set; } = string.Empty;
    public List<WeaponQualityDto> Qualities { get; set; } = [];
    public Guid WeaponTypeId { get; set; }
}