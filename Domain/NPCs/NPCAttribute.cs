using Domain.Characters;

namespace Domain.NPCs;
public class NPCAttribute
{
    public Guid NPCId { get; set; }
    public NPC NPC { get; set; } = null!;
    public Guid AttributeId { get; set; }
    public CAttribute Attribute { get; set; } = null!;
    public int Level { get; set; }
}
