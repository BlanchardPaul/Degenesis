namespace Degenesis.Shared.DTOs.Characters;
public class AttributeDto : AttributeCreateDto
{
    public Guid Id { get; set; }
}
public class AttributeCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string Abbreviation { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsFocusOriented { get; set; }
}
