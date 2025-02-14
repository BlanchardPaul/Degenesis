namespace Domain.Characters;
public class RankPrerequisite
{
    public Guid Id { get; set; }

    public Guid RankId { get; set; }
    public Rank Rank { get; set; } = new();

    public Guid AttributeRequiredId { get; set; }
    public CAttribute AttributeRequired { get; set; } = new();

    public Guid? SkillRequiredId { get; set; }
    public Skill? SkillRequired { get; set; }
    public int SumRequired { get; set; }
}