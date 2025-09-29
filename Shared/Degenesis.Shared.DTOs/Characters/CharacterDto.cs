using Degenesis.Shared.DTOs.Rooms;

namespace Degenesis.Shared.DTOs.Characters;
public class CharacterCreateDto
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public int Height { get; set; }
    public int Weight { get; set; }
    public string Sex { get; set; } = string.Empty;
    public int MaxEgo { get; set; } = 2;
    public int MaxSporeInfestation { get; set; } = 2;
    public int MaxFleshWounds { get; set; } = 2;
    public int MaxTrauma { get; set; } = 2;
    public bool IsFocusOriented { get; set; }
    public Guid CultId { get; set; }
    public Guid CultureId { get; set; }
    public Guid ConceptId { get; set; }
    public Guid IdRoom { get; set; }
    public Guid RankId { get; set; }
    public List<CharacterAttributeDto> Attributes { get; set; } = [];
    public List<CharacterSkillDto> Skills { get; set; } = [];
    public List<CharacterBackgroundDto> Backgrounds { get; set; } = [];
    public List<CharacterPotentialDto> Potentials { get; set; } = []; 
}

public class CharacterDto : CharacterCreateDto
{
    public Guid Id { get; set; }

    public int DinarMoney { get; set; }
    public int ChroniclerMoney { get; set; }
    public int Ego { get; set; }
    public int CurrentSporeInfestation { get; set; }
    public int PermanentSporeInfestation { get; set; }
    public int FleshWounds { get; set; }
    public int Trauma { get; set; }
    public int PassiveDefense { get; set; }
    public int Experience { get; set; }
    public string Notes { get; set; } = string.Empty;

    public Guid IdApplicationUser { get; set; }

    public CultDto Cult { get; set; } = new();
    public CultureDto Culture { get; set; } = new();
    public ConceptDto Concept { get; set; } = new();
    public RoomDto Room { get; set; } = new();
    public RankDto Rank { get; set; } = new();

    public new List<CharacterAttributeDto> Attributes { get; set; } = [];
    public new List<CharacterSkillDto> Skills { get; set; } = [];
    public new List<CharacterBackgroundDto> Backgrounds { get; set; } = [];
}