using Degenesis.Shared.DTOs.Characters.CRUD;

namespace Degenesis.Shared.DTOs.Protections;
public class ProtectionDto : ProtectionCreateDto
{
    public Guid Id { get; set; }   
}

public class ProtectionCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int? Armor { get; set; }
    public string Qualities { get; set; } = string.Empty;
    public int Slots { get; set; } = 0;
    public string Defense { get; set; } = string.Empty;
    public string Attack { get; set; } = string.Empty;
    public int Encumbrance { get; set; } = 0;
    public int TechLevel { get; set; } = 1;
    public string Value { get; set; } = string.Empty;
    public string Resources { get; set; } = string.Empty;
    public List<CultDto> Cults { get; set; } = new();
}
