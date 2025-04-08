using Domain.Users;

namespace Domain.Rooms;
public class UserRoom
{
    public Guid Id { get; set; }
    public Guid IdApplicationUser { get; set; }
    public ApplicationUser ApplicationUser { get; set; } = new();
    public Guid IdRoom { get; set; }
    public Room Room { get; set; } = new();
    public bool IsGM { get; set; }
    public bool InvitationAccepted { get; set; }
}
