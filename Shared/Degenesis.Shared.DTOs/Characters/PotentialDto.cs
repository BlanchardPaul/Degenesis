namespace Degenesis.Shared.DTOs.Characters;
public class PotentialDto : PotentialCreateDto
{
    public Guid Id { get; set; }
    public CultDto? Cult { get; set; } = new();
}

public class PotentialCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid? CultId { get; set; }
    public List<PotentialPrerequisiteDto> Prerequisites { get; set; } = [];
}
