using Degenesis.Shared.DTOs.Characters.CRUD;

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
    public CultDto Cult { get; set; } = new();
    public CultureDto Culture { get; set; } = new();
    public ConceptDto Concept { get; set; } = new();
    public RankDto Rank { get; set; } = new();

    public List<CharacterAttributeDisplayDto> Attributes { get; set; } = [];
    public List<CharacterSkillDisplayDto> Skills { get; set; } = [];
    public List<CharacterBackgroundDisplayDto> Backgrounds { get; set; } = [];
    public List<CharacterPotentialDisplayDto> Potentials { get; set; } = [];
}