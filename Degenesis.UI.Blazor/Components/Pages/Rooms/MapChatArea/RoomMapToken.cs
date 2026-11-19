namespace Degenesis.UI.Blazor.Components.Pages.Rooms.MapChatArea;

public sealed class RoomMapToken
{
    public Guid MemberId { get; init; }
    public bool IsNpc { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? AvatarName { get; init; }
    public double X { get; set; }
    public double Y { get; set; }

    public string Key => $"{(IsNpc ? "npc" : "character")}:{MemberId}";
}
