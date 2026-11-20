namespace Degenesis.Shared.DTOs.Rooms;

public class RoomMapTokenDto
{
    public Guid MemberId { get; set; }
    public bool IsNpc { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? AvatarName { get; set; }
    public double X { get; set; }
    public double Y { get; set; }
}
