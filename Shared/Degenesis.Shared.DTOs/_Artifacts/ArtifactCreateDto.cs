namespace Degenesis.Shared.DTOs.Artifacts;

public class ArtifactCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string EnergyStorage { get; set; } = string.Empty;
    public int Magazine { get; set; } = 0;
    public int Encumbrance { get; set; } = 0;
    public int Activation { get; set; } = 0;
    public int Value { get; set; } = 0;
}