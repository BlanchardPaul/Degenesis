using Domain.NPCs;

namespace Domain.Protections;
public class NPCProtection
{
    public Guid Id { get; set; }
    public Guid NPCId { get; set; }
    public NPC NPC { get; set; } = new();
    public Guid ProtectionId { get; set; }
    public Protection Protection { get; set; } = new();
    // The Used... will be used to display Used.../Max...
    public int UsedConnectors { get; set; } = 0;
    public int UsedSlots { get; set; } = 0;
}