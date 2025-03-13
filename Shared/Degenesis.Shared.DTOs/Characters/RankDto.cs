namespace Degenesis.Shared.DTOs.Characters;

public class RankCreateDto
{
    public Guid CultId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Equipment { get; set; } = string.Empty;
    public List<RankPrerequisiteDto> Prerequisites { get; set; } = [];
}

public class RankDto : RankCreateDto
{
    public Guid Id { get; set; }
    public CultDto Cult { get; set; } = new();
}