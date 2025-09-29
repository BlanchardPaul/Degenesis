namespace Domain.Characters;

public class CAttribute
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Abbreviation { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsFocusOriented { get; set; }
    public List<Skill> Skills { get; set; } = [];
}
