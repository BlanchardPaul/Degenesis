namespace Degenesis.Shared.DTOs.Characters;
public class PotentialPrerequisiteCreateDto
{
    public Guid? AttributeRequiredId { get; set; }
    public Guid? SkillRequiredId { get; set; }
    public int? SumRequired { get; set; }
    public Guid? BackgroundRequiredId { get; set; }
    public int? BackgroundLevelRequired { get; set; }
    public bool IsBackgroundPrerequisite { get; set; }
    public Guid? RankRequiredId { get; set; }
    public bool IsRankPrerequisite { get; set; }
}

public class PotentialPrerequisiteDto : PotentialPrerequisiteCreateDto
{
    public Guid Id { get; set; }
    public AttributeDto? AttributeRequired { get; set; }
    public SkillDto? SkillRequired { get; set; }
    public BackgroundDto? BackgroundRequired { get; set; }
    public RankDto? RankRequired { get; set; }
}