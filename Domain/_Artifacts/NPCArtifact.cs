using Domain.NPCs;

namespace Domain._Artifacts;

public class NPCArtifact
{
    public Guid Id { get; set; }
    public Guid NPCId { get; set; }
    public NPC NPC { get; set; } = new();
    public Guid ArtifactId { get; set; }
    public Artifact Artifact { get; set; } = new();
    public int ChargeInMagazine { get; set; }
}