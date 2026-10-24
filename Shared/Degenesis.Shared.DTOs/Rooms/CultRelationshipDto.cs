namespace Degenesis.Shared.DTOs.Rooms;
public class CultRelationshipDto
{
    public Guid Id { get; set; }
    public bool IsDiscovered { get; set; }
    public int Relationship { get; set; }
    public Guid IdCult { get; set; }
    public string CultName { get; set; } = string.Empty;
    public string? CultImageName { get; set; }
}