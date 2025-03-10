namespace Degenesis.Shared.DTOs.Characters;
public class CultCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<SkillDto> BonusSkills { get; set; } = new();
}

public class CultDto : CultCreateDto
{
    public Guid Id { get; set; }
}