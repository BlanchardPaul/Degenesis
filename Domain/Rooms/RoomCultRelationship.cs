using Domain.Characters;

namespace Domain.Rooms;
public class RoomCultRelationship
{
    public Guid Id { get; set; }
    public bool IsDiscovered { get; set; }
    //Relationship from -6 to +6 according to cult view of the group
    public int Relationship { get; set; }
    public Guid IdCult { get; set; }
    public Cult Cult { get; set; } = null!;
    public Guid IdRoom { get; set; }
    public Room Room { get; set; } = null!;
}
