namespace Domain.Characters;
public class Potential
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    // Can depend on to many things, so just a string
    public string Preriquisite { get; set; } = string.Empty;

    public Guid? CultId { get; set; }
    public Cult? Cult { get; set; } = new();
}