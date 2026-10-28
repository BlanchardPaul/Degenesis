namespace Degenesis.Shared.DTOs;
public class BaseNPCDto : BaseNPCCreateDto
{
    public Guid Id { get; set; }
    public List<BaseNPCAttributeDto> Attributes { get; set; } = [];
    public List<BaseNPCSkillDto> Skills { get; set; } = [];
}

public class BaseNPCCreateDto
{
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
}

public class BaseNPCAttributeDto
{
    public Guid AttributeId { get; set; }
    public string AttributeName { get; set; } = string.Empty;
    public int Level { get; set; }
}

public class BaseNPCSkillDto
{
    public Guid SkillId { get; set; }
    public string SkillName { get; set; } = string.Empty;
    public Guid AttributeId { get; set; }
    public int Level { get; set; }
}