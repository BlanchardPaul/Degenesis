namespace Degenesis.Shared.DTOs.Characters;

public class RankPrerequisiteCreateDto
{
    public Guid? AttributeRequiredId { get; set; }
    public Guid? SkillRequiredId { get; set; }
    public int? SumRequired { get; set; }
    public Guid? BackgroundRequiredId { get; set; }
    public int? BackgroundLevelRequired { get; set; }
    public bool IsBackgroundPrerequisite { get; set; }
}

public class RankPrerequisiteDto : RankPrerequisiteCreateDto
{
    public Guid Id { get; set; }
    public AttributeDto? AttributeRequired { get; set; }
    public SkillDto? SkillRequired { get; set; }
    public BackgroundDto? BackgroundRequired { get; set; }
}