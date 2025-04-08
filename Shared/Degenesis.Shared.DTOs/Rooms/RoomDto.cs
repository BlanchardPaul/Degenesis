namespace Degenesis.Shared.DTOs.Rooms;

public class RoomDto : RoomCreateDto
{
    public Guid Id { get; set; }
}

public class RoomCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public class RoomDisplayDto : RoomDto
{
    public string GMName { get; set; } = string.Empty;
    public List<string> Players { get; set; } = [];
    public List<string> Pendings { get; set; } = [];
    public bool IsPendingForCurrentUser { get; set; }
    public bool IsCurrentUserGM { get; set; }
}