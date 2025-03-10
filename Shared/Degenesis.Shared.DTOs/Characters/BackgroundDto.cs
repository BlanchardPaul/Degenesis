namespace Degenesis.Shared.DTOs.Characters;
public class BackgroundDto : BackgroundCreateDto
{
    public Guid Id { get; set; }
}
public class BackgroundCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
