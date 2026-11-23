namespace Degenesis.Shared.DTOs.Rooms;

public class RoomMapDrawingDto
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Color { get; set; } = "#f44336";
    public int StrokeWidth { get; set; } = 4;
    public List<RoomMapPointDto> Points { get; set; } = [];
}

public class RoomMapPointDto
{
    public double X { get; set; }
    public double Y { get; set; }
}

public class RoomMapStateDto
{
    public List<RoomMapTokenDto> Tokens { get; set; } = [];
    public List<RoomMapDrawingDto> Drawings { get; set; } = [];
}
