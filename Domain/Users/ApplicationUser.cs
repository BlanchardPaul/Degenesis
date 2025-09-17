using Domain.Characters;
using Domain.Rooms;
using Microsoft.AspNetCore.Identity;

namespace Domain.Users;
public class ApplicationUser : IdentityUser<Guid>
{
    public List<UserRoom> UserRooms { get; set; } = [];
    public List<Character> Characters { get; set; } = [];
}