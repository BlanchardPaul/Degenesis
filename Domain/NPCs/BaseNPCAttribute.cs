using Domain.Characters;

namespace Domain.NPCs;
public class BaseNPCAttribute
{
    public Guid BaseNPCId { get; set; }
    public BaseNPC BaseNPC { get; set; } = null!;
    public Guid AttributeId { get; set; }
    public CAttribute Attribute { get; set; } = null!;
    public int Level { get; set; }
}