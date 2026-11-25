using Microsoft.AspNetCore.Identity;

namespace Domain.Users;
public class Role : IdentityRole<Guid>
{
    public const string Admin = "Admin";
    public const string GameMaster = "GameMaster";
    public const string Player = "Player";
}
