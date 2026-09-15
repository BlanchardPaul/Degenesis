using Domain.Characters;

namespace Domain.Protections;
public class Protection
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    // If Armor is null it's a shield, else armor value
    public int? Armor { get; set; }

    public string Qualities { get; set; } = string.Empty;

    // Number of innert devices one can connect on the armor (steel plates, camo, ...)
    public int Slots { get; set; } = 0;
    public string Defense { get; set; } = string.Empty;
    public string Attack { get; set; } = string.Empty;
    public int Encumbrance { get; set; } = 0;
    public int TechLevel { get; set; } = 1;
    public string Value { get; set; } = string.Empty;
    public string Resources { get; set; } = string.Empty;
    public List<Cult> Cults { get; set; } = [];
}