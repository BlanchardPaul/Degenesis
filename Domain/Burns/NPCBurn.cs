using Domain.NPCs;

namespace Domain.Burns;
public class NPCBurn
{
    public Guid NPCId { get; set; }
    public NPC NPC { get; set; } = new();
    public Guid BurnId { get; set; }
    public Burn Burn { get; set; } = new();
    public int Quantity { get; set; } = 1;
}