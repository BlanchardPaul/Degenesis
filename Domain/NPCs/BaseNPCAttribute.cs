using Domain.Characters;

namespace Domain.NPCs;
public class BaseNPCAttribute
{
    public Guid NPCId { get; set; }
    public BaseNPC NPC { get; set; } = new();
    public Guid AttributeId { get; set; }
    public CAttribute Attribute { get; set; } = new();
    public int Level { get; set; }
}