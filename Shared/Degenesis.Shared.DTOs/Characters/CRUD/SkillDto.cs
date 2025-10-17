namespace Degenesis.Shared.DTOs.Characters.CRUD;

public class SkillCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty; 
    public string Abbreviation { get; set; } = string.Empty;
    public bool IsFocusOriented { get; set; }
    public Guid CAttributeId { get; set; }
}

public class SkillDto : SkillCreateDto
{
    public Guid Id { get; set; }
    public AttributeDto CAttribute { get; set; } = new();
}