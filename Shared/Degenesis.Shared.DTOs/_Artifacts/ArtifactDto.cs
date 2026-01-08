namespace Degenesis.Shared.DTOs._Artifacts;

public class ArtifactDto : ArtifactCreateDto
{
    public Guid Id { get; set; }
}
public class ArtifactCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string EnergyStorage { get; set; } = string.Empty;
    public int Magazine { get; set; } = 0;
    public int Encumbrance { get; set; } = 0;
    public int Activation { get; set; } = 0;
    public string Value { get; set; } = string.Empty;
}