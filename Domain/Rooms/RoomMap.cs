namespace Domain.Rooms;

public class RoomMap
{
    public Guid Id { get; set; }
    public Guid IdRoom { get; set; }
    public string Name { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public int Width { get; set; }
    public int Height { get; set; }
    public int GridCellSize { get; set; } = 100;
    public bool IsGridVisible { get; set; } = true;
    public string DrawingsJson { get; set; } = "[]";
    public Room Room { get; set; } = null!;
    public List<RoomMapToken> Tokens { get; set; } = [];
}
