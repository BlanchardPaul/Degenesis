namespace Domain.Characters;
public class Skill
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid AttributeId { get; set; }
    public CAttribute Attribute { get; set; } = new();
}