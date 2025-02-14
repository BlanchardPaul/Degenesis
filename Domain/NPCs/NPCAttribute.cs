using Domain.Characters;

namespace Domain.NPCs;
public class NPCAttribute
{
    public Guid NPCId { get; set; }
    public NPC NPC { get; set; } = new();
    public Guid AttributeId { get; set; }
    public CAttribute Attribute { get; set; } = new();
    public int Level { get; set; }
}