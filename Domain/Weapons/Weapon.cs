using Domain.Characters;

namespace Domain.Weapons;
public class Weapon
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Caliber { get; set; } = string.Empty;
    public string Handling { get; set; } = string.Empty;
    public string Distance { get; set; } = string.Empty;

    // Used for static damages
    public int? Damage { get; set; }

    // Used for damages that depends from character attributes and skills
    public Guid? AttributeId { get; set; }
    public CAttribute? Attribute { get; set; }
    public Guid? SkillId { get; set; }
    public Skill? Skill { get; set; }

    // If the weapon is a melee one => damage is Damage + (CharacterStrengh+CharacterForce)/2 we put the 2 in CharacterAttributeModifier
    public int? CharacterAttributeModifier { get; set; }
    public int Magazine { get; set; } = 0;
    public int Encumbrance { get; set; } = 0;
    public int TechLevel { get; set; } = 1;
    public int Slots { get; set; } = 0;
    public string Value { get; set; } = string.Empty;
    public string Resources { get; set; } = string.Empty;
    public List<WeaponQuality> Qualities { get; set; } = [];
    public Guid WeaponTypeId { get; set; }
    public WeaponType WeaponType { get; set; } = new();
    public List<Cult> Cults { get; set; } = [];
}