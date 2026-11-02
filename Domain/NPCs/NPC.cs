using Domain.Rooms;

namespace Domain.NPCs;
public class NPC
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Initiative { get; set; } = string.Empty;
    public string Specialty { get; set; } = string.Empty;
    public string Attack { get; set; } = string.Empty;
    public string Defense { get; set; } = string.Empty;
    public string Movement { get; set; } = string.Empty;
    public string Armor { get; set; } = string.Empty;
    public string Equipment { get; set; } = string.Empty;
    public string Condition { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public List<NPCAttribute> NPCAttributes { get; set; } = [];
    public List<NPCSkill> NPCSkills { get; set; } = [];
    public Guid IdRoom { get; set; }
    public Room Room { get; set; } = null!;
}
