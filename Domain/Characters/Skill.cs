namespace Domain.Characters;
public class Skill
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public Guid CAttributeId { get; set; }

    public CAttribute CAttribute { get; set; } = null!;
}