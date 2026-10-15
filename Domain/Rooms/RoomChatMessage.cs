namespace Domain.Rooms;

public class RoomChatMessage
{
    public Guid Id { get; set; }
    public Guid IdRoom { get; set; }
    public string Sender { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public DateTime SentAtUtc { get; set; } = DateTime.UtcNow;
    public string? RollResultJson { get; set; }
    public Room? Room { get; set; }
}