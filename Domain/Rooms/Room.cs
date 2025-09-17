using Domain.Characters;

namespace Domain.Rooms;
public class Room
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<UserRoom> UserRooms { get; set; } = [];
    public List<Character> Characters { get; set; } = [];
}
