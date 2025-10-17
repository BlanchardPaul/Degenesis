namespace Degenesis.Shared.DTOs.Characters.CRUD;

public class RankCreateDto
{
    public Guid CultId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Equipment { get; set; } = string.Empty;
    public Guid? ParentRankId { get; set; }
    public List<RankPrerequisiteDto> Prerequisites { get; set; } = [];
}

public class RankDto : RankCreateDto
{
    public Guid Id { get; set; }
    public CultDto Cult { get; set; } = new();
    public RankDto? ParentRank { get; set; }
}