namespace Domain.Protections;
public class Protection
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    // If Armor is null it's a shield, else armor value
    public int? Armor { get; set; }

    public List<ProtectionQuality> Qualities { get; set; } = [];

    // Bonus weight the character can carry
    public int Stockage { get; set; } = 0;

    // Number of innert devices one can connect on the armor (steel plates, camo, ...)
    public int Slots { get; set; } = 0;

    // Number of technologic devices one can connect on the armor (HUD, auto-injection,...)
    public int Connectors { get; set; } = 0;
    public string Consuption { get; set; } = string.Empty;
    public string Defense { get; set; } = string.Empty;
    public string Attack { get; set; } = string.Empty;
    public int Encumbrance { get; set; } = 0;
    public int TechLevel { get; set; } = 1;
    public int Value { get; set; } = 0;

    // Whatever you want, positives/negatives bonus, cost of keep, if a faction will attack on sight if it sees this garment, ...)
    public string Resources { get; set; } = string.Empty;
}