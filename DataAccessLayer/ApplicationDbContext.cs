using Domain._Artifacts;
using Domain.Burns;
using Domain.Characters;
using Domain.Equipments;
using Domain.NPCs;
using Domain.Protections;
using Domain.Rooms;
using Domain.Users;
using Domain.Vehicles;
using Domain.Weapons;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DataAccessLayer;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, Role, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Artifact> Artifacts { get; set; } = null!;
    public DbSet<CAttribute> Attributes { get; set; } = null!;
    public DbSet<Background> Backgrounds { get; set; } = null!;
    public DbSet<Burn> Burns { get; set; } = null!;
    public DbSet<Character> Characters { get; set; } = null!;
    public DbSet<CharacterArtifact> CharacterArtifacts { get; set; } = null!;
    public DbSet<CharacterAttribute> CharacterAttributes { get; set; } = null!;
    public DbSet<CharacterBurn> CharacterBurns { get; set; } = null!;
    public DbSet<CharacterBackground> CharacterBackgrounds { get; set; } = null!;
    public DbSet<CharacterEquipment> CharacterEquipments { get; set; } = null!;
    public DbSet<CharacterPotential> CharacterPotentials { get; set; } = null!;
    public DbSet<CharacterProtection> CharacterProtections { get; set; } = null!;
    public DbSet<CharacterSkill> CharacterSkills { get; set; } = null!;
    public DbSet<CharacterVehicle> CharacterVehicles { get; set; } = null!;
    public DbSet<CharacterWeapon> CharacterWeapons { get; set; } = null!;
    public DbSet<Concept> Concepts { get; set; } = null!;
    public DbSet<Cult> Cults { get; set; } = null!;
    public DbSet<Culture> Cultures { get; set; } = null!;
    public DbSet<Equipment> Equipments { get; set; } = null!;
    public DbSet<EquipmentType> EquipmentTypes { get; set; } = null!;
    public DbSet<NPC> NPCs { get; set; } = null!;
    public DbSet<NPCArtifact> NPCArtifacts { get; set; } = null!;
    public DbSet<NPCAttribute> NPCAttributes { get; set; } = null!;
    public DbSet<NPCBurn> NPCBurns { get; set; } = null!;
    public DbSet<NPCEquipment> NPCEquipments { get; set; } = null!;
    public DbSet<NPCPotential> NPCPotentials { get; set; } = null!;
    public DbSet<NPCProtection> NPCProtections { get; set; } = null!;
    public DbSet<NPCSkill> NPCSkills { get; set; } = null!;
    public DbSet<NPCWeapon> NPCWeapons { get; set; } = null!;
    public DbSet<Potential> Potentials { get; set; } = null!;
    public DbSet<PotentialPrerequisite> PotentialPrerequisites { get; set; } = null!;
    public DbSet<Protection> Protections { get; set; } = null!;
    public DbSet<ProtectionQuality> ProtectionQualities { get; set; } = null!;
    public DbSet<Rank> Ranks { get; set; } = null!;
    public DbSet<RankPrerequisite> RankPrerequisites { get; set; } = null!;
    public DbSet<Room> Rooms { get; set; } = null!;
    public DbSet<Skill> Skills { get; set; } = null!;
    public DbSet<UserRoom> UserRooms { get; set; } = null!;
    public DbSet<Vehicle> Vehicles { get; set; } = null!;
    public DbSet<VehicleType> VehicleTypes { get; set; } = null!;
    public DbSet<Weapon> Weapons { get; set; } = null!;
    public DbSet<WeaponQuality> WeaponQualities { get; set; } = null!;
    public DbSet<WeaponType> WeaponTypes { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);

        // Set precision for decimal properties
        IEnumerable<IMutableProperty> decimalProperties = builder.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?));
        foreach (var property in decimalProperties)
        {
            property.SetPrecision(19);
            property.SetScale(4);
        }
    }
}
