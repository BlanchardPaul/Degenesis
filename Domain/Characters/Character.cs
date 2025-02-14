using Domain.Artifacts;
using Domain.Burns;
using Domain.Equipments;
using Domain.Protections;
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
    public int Ego { get; set; } = 2;
    public int CurrentSporeInfestation { get; set; } = 0;
    public int MaxSporeInfestation { get; set; } = 2;
    public int PermanentSporeInfestation { get; set; } = 0;
    public int FleshWounds { get; set; } = 2;
    public int Trauma { get; set; } = 0;
    public int PassiveDefense { get; set; } = 1;
    public int Experience { get; set; }
    public string Notes { get; set; } = string.Empty;
    public Guid CultId { get; set; }
    public Cult Cult { get; set; } = new();
    public Guid CultureId { get; set; }
    public Culture Culture { get; set; } = new();
    public Guid ConceptId { get; set; }
    public Concept Concept { get; set; } = new();
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
