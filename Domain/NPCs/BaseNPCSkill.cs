using Domain.Characters;

namespace Domain.NPCs;
public class BaseNPCSkill
{
    public Guid NPCId { get; set; }
    public BaseNPC NPC { get; set; } = new();
    public Guid SkillId { get; set; }
    public Skill Skill { get; set; } = new();
    public int Level { get; set; }
}
