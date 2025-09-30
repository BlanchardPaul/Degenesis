namespace Degenesis.Shared.DTOs._Artifacts;
public class CharacterArtifactCreateDto
{
    public Guid Id { get; set; }
    public Guid CharacterId { get; set; }
    public Guid ArtifactId { get; set; }
}

public class CharacterArtifactDto : CharacterArtifactCreateDto
{
    public ArtifactDto Artifact { get; set; } = new();
    public int ChargeInMagazine { get; set; }
    // No need for the characterDto since this Dto only servs to display the artifacts in the character sheet
}
