using Domain.Characters;

namespace Domain._Artifacts;

public class CharacterArtifact
{
    public Guid Id { get; set; }
    public Guid CharacterId { get; set; }
    public Character Character { get; set; } = new();
    public Guid ArtifactId { get; set; }
    public Artifact Artifact { get; set; } = new();
    public int ChargeInMagazine { get; set; }
}