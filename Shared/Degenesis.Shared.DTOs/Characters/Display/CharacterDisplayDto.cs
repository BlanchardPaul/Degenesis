using Degenesis.Shared.DTOs.Characters.CRUD;
using Degenesis.Shared.DTOs.Characters.CRUD.Inventory;
using System.Threading;

namespace Degenesis.Shared.DTOs.Characters.Display;
public class CharacterDisplayDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public int Height { get; set; }
    public int Weight { get; set; }
    public string Sex { get; set; } = string.Empty;
    public int DinarMoney { get; set; }
    public int ChroniclerMoney { get; set; }
    public int MaxEgo { get; set; }
    public int Ego { get; set; }
    public int CurrentSporeInfestation { get; set; }
    public int MaxSporeInfestation { get; set; }
    public int PermanentSporeInfestation { get; set; }
    public int MaxFleshWounds { get; set; }
    public int FleshWounds { get; set; }
    public int MaxTrauma { get; set; }
    public int Trauma { get; set; }
    public int PassiveDefense { get; set; }
    public int Experience { get; set; }
    public string Notes { get; set; } = string.Empty;
    public string InventoryNotes { get; set; } = string.Empty;
    public bool IsFocusOriented { get; set; }
    public CultDto Cult { get; set; } = new();
    public CultureDto Culture { get; set; } = new();
    public ConceptDto Concept { get; set; } = new();
    public RankDto Rank { get; set; } = new();

    public List<CharacterAttributeDisplayDto> Attributes { get; set; } = [];
    public List<CharacterSkillDisplayDto> Skills { get; set; } = [];
    public List<CharacterBackgroundDisplayDto> Backgrounds { get; set; } = [];
    public List<CharacterPotentialDisplayDto> Potentials { get; set; } = [];
    public List<CharacterArtifactDto> Artifacts { get; set; } = [];
    public List<CharacterBurnDto> Burns { get; set; } = [];
    public List<CharacterEquipmentDto> Equipments { get; set; } = [];
    public List<CharacterProtectionDto> Protections { get; set; } = [];
    public List<CharacterVehicleDto> Vehicles { get; set; } = [];
    public List<CharacterWeaponDto> Weapons { get; set; } = [];
}