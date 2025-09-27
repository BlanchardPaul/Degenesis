namespace Domain.Characters;
public class Potential
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid? CultId { get; set; }
    public Cult? Cult { get; set; } = new();
    public List<PotentialPrerequisite> Prerequisites { get; set; } = [];

}