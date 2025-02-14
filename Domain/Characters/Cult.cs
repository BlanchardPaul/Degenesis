namespace Domain.Characters;

public class Cult
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    // Skills wich can get a +1
    public List<Skill> BonusSkills { get; set; } = [];
}