using Domain.Characters;

namespace Domain.NPCs;
public class BaseNPCSkill
{
    public Guid BaseNPCId { get; set; }
    public BaseNPC BaseNPC { get; set; } = null!;
    public Guid SkillId { get; set; }
    public Skill Skill { get; set; } = null!;
    public int Level { get; set; }
}
