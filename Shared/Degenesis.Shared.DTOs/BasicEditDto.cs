namespace Degenesis.Shared.DTOs;
public class IdIntEditDto
{
    public Guid Id { get; set; }
    public int Value { get; set; }
}

public class IdGuidEditDto
{
    public Guid Id { get; set; }
    public Guid Value { get; set; }
}


public class IdStringEditDto
{
    public Guid Id { get; set; }
    public string Value { get; set; } = string.Empty;
}
