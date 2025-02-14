namespace Domain.Characters;
public class Concept
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    // Attributes wich can get a +1
    public Guid BonusAttributeId { get; set; }
    public CAttribute BonusAttribute { get; set; } = new();

    // Skills wich can get a +1
    public List<Skill> BonusSkills { get; set; } = [];
}