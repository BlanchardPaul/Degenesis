namespace Domain.Rooms;

public class RoomMapToken
{
    public Guid Id { get; set; }
    public Guid IdRoomMap { get; set; }
    public Guid MemberId { get; set; }
    public bool IsNpc { get; set; }
    public double X { get; set; }
    public double Y { get; set; }
    public RoomMap RoomMap { get; set; } = null!;
}
