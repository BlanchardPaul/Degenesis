using Domain.Characters;

namespace Domain.NPCs;
public class NPCSkill
{
    public Guid NPCId { get; set; }
    public NPC NPC { get; set; } = new();
    public Guid SkillId { get; set; }
    public Skill Skill { get; set; } = new();
    public int Level { get; set; }
}
