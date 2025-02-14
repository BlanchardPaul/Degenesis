using Domain.Characters;

namespace Domain.NPCs;

public class NPCPotential
{
    public Guid NPCId { get; set; }
    public NPC NPC { get; set; } = new();
    public Guid PotentialId { get; set; }
    public Potential Potential { get; set; } = new();
    public int Level { get; set; }
}