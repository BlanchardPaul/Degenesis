namespace Domain.Characters;
public class ConceptSkill
{
    public Guid ConceptId { get; set; }
    public Concept Concept { get; set; } = new();

    public Guid SkillId { get; set; }
    public Skill Skill { get; set; } = new();
}