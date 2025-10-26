namespace Degenesis.Shared.DTOs.Characters.CRUD;
public class CharacterBasicInfosEditDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public int Height { get; set; }
    public int Weight { get; set; }
    public string Sex { get; set; } = string.Empty;
    public bool IsFocusOriented { get; set; }
}

public class CharacterIntValueEditDto
{
    public Guid Id { get; set; }
    public int Value { get; set; }
}

public class CharacterGuidValueEditDto
{
    public Guid Id { get; set; }
    public Guid Value { get; set; }
}


public class CharacterStringValueEditDto
{
    public Guid Id { get; set; }
    public string Value { get; set; } = string.Empty;
}