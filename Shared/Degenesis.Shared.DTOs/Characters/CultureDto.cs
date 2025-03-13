namespace Degenesis.Shared.DTOs.Characters;
public class CultureDto : CultureCreateDto
{
    public Guid Id { get; set; }
}

public class CultureCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<CultDto> AvailableCults { get; set; } = [];
    public List<AttributeDto> BonusAttributes { get; set; } = [];
    public List<SkillDto> BonusSkills { get; set; } = [];
}
