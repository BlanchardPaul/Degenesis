using Domain._Artifacts;
using Domain.Burns;
using Domain.Equipments;
using Domain.Protections;
using Domain.Rooms;
using Domain.Users;
using Domain.Vehicles;
using Domain.Weapons;

namespace Domain.Characters;

public class Character
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public int Height { get; set; }
    public int Weight { get; set; }
    public string Sex { get; set; } = string.Empty;
    public int DinarMoney { get; set; }
    public int ChroniclerMoney { get; set; }
    public int MaxEgo { get; set; } = 2;
    public int Ego { get; set; } = 2;
    public int CurrentSporeInfestation { get; set; } = 0;
    public int MaxSporeInfestation { get; set; } = 2;
    public int PermanentSporeInfestation { get; set; } = 0;
    public int MaxFleshWounds { get; set; } = 2;
    public int FleshWounds { get; set; } = 2;
    public int MaxTrauma { get; set; } = 2;
    public int Trauma { get; set; } = 2;
    public int PassiveDefense { get; set; } = 1;
    public int Experience { get; set; }
    public string Notes { get; set; } = string.Empty;
    public bool IsFocusOriented { get; set; }
    public Guid CultId { get; set; }
    public Cult Cult { get; set; } = new();
    public Guid CultureId { get; set; }
    public Culture Culture { get; set; } = new();
    public Guid ConceptId { get; set; }
    public Concept Concept { get; set; } = new();
    public Guid IdRoom { get; set; }
    public Room Room { get; set; } = new();
    public Guid RankId { get; set; }
    public Rank Rank { get; set; } = new();
    public Guid IdApplicationUser { get; set; }
    public ApplicationUser ApplicationUser { get; set; } = new();
    public List<CharacterBackground> CharacterBackgrounds { get; set; } = [];
    public List<CharacterBurn> CharacterBurns { get; set; } = [];
    public List<CharacterAttribute> CharacterAttributes { get; set; } = [];
    public List<CharacterSkill> CharacterSkills { get; set; } = [];
    public List<CharacterPotential> CharacterPontentials { get; set; } = [];
    public List<CharacterProtection> CharacterProtections { get; set; } = [];
    public List<CharacterEquipment> CharacterEquipments { get; set; } = [];
    public List<CharacterArtifact> CharacterArtifacts { get; set; } = [];
    public List<CharacterVehicle> CharacterVehicles { get; set; } = [];
    public List<CharacterWeapon> CharacterWeapons { get; set; } = [];
}
