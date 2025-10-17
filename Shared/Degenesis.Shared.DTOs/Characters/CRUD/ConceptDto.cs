namespace Degenesis.Shared.DTOs.Characters.CRUD;

public class ConceptCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid BonusAttributeId { get; set; }
    public AttributeDto BonusAttribute { get; set; } = new();
    public List<SkillDto> BonusSkills { get; set; } = [];
}

public class ConceptDto : ConceptCreateDto
{
    public Guid Id { get; set; }
}
   