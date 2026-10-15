namespace Degenesis.Shared.DTOs.Rooms;
public class RoomChatMessageDto
{
    public string Sender { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public DateTime SentAtUtc { get; set; }
    public DiceRollResultDto? RollResult { get; set; }
}
