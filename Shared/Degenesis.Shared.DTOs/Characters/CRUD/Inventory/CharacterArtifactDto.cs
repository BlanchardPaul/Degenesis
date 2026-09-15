using Degenesis.Shared.DTOs._Artifacts;

namespace Degenesis.Shared.DTOs.Characters.CRUD.Inventory;
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
}
