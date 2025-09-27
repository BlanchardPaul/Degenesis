namespace Domain.Characters;
public class PotentialPrerequisite
{
    public Guid Id { get; set; }
    public Guid? AttributeRequiredId { get; set; }
    public CAttribute? AttributeRequired { get; set; }
    public Guid? SkillRequiredId { get; set; }
    public Skill? SkillRequired { get; set; }
    public int? SumRequired { get; set; }
    public Guid? BackgroundRequiredId { get; set; }
    public Background? BackgroundRequired { get; set; }
    public int? BackgroundLevelRequired { get; set; }
    public bool IsBackgroundPrerequisite { get; set; }
    public Guid? RankRequiredId { get; set; }
    public Rank? RankRequired { get; set; }
    public bool IsRankPrerequisite { get; set; }
}
