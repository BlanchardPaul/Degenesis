namespace Domain.Characters;

public class Culture
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    // Available Cults in the Culture
    public List<Cult> AvailableCults { get; set; } = [];

    // Attributes wich can get a +1
    public List<CAttribute> BonusAttributes { get; set; } = [];

    // Skills wich can get a +1
    public List<Skill> BonusSkills { get; set; } = [];
}