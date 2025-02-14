using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    // Used for damages that depends from character attributes
    public Guid? AttributeId { get; set; }
    public Characters.CAttribute? Attribute { get; set; }
    // If the weapon damage is CharacterStrengh/2 we put 0.5 in base since we will multiply CharacterAttribute*CharacterAttributeModifier
    public float? CharacterAttibuteModifier { get; set; }

    public int Magazine { get; set; } = 0;
    public int Encumbrance { get; set; } = 0;
    public int TechLevel { get; set; } = 1;
    public int Slots { get; set; } = 0;
    public int Value { get; set; } = 0;
    public string Resources { get; set; } = string.Empty;
    public List<WeaponQuality> Qualities { get; set; } = [];
    public Guid WeaponTypeId { get; set; }
    public WeaponType WeaponType { get; set; } = new();
}