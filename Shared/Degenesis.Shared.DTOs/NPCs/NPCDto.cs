namespace Degenesis.Shared.DTOs.NPCs;
public class NPCDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Initiative { get; set; } = string.Empty;
    public string Specialty { get; set; } = string.Empty;
    public string Attack { get; set; } = string.Empty;
    public string Defense { get; set; } = string.Empty;
    public string Movement { get; set; } = string.Empty;
    public string Armor { get; set; } = string.Empty;
    public string Equipment { get; set; } = string.Empty;
    public string Condition { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public Guid IdRoom { get; set; }
    public List<NPCAttributeDto> Attributes { get; set; } = [];
    public List<NPCSkillDto> Skills { get; set; } = [];
}

public class NPCAttributeDto
{
    public Guid AttributeId { get; set; }
    public string AttributeName { get; set; } = string.Empty;
    public int Level { get; set; }
}

public class NPCSkillDto
{
    public Guid SkillId { get; set; }
    public string SkillName { get; set; } = string.Empty;
    public Guid AttributeId { get; set; }
    public int Level { get; set; }
}