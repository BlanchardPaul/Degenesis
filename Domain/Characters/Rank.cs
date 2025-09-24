namespace Domain.Characters;
public class Rank
{
    public Guid Id { get; set; }
    public Guid CultId { get; set; }
    public Cult Cult { get; set; } = new();
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Equipment { get; set; } = string.Empty;

    // List of prerequisites, only one need to be fulfilled to pass the rank.
    public List<RankPrerequisite> Prerequisites { get; set; } = [];

    // Optional parent rank (previous rank in progression)
    public Guid? ParentRankId { get; set; }
    public Rank? ParentRank { get; set; }
}