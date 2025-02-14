using Domain.Artifacts;
using Domain.Burns;
using Domain.Equipments;
using Domain.Protections;
using Domain.Weapons;

namespace Domain.NPCs;

public class NPC
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Height { get; set; }
    public int Weight { get; set; }
    public int Ego { get; set; } = 2;
    public int FleshWounds { get; set; } = 2;
    public int Trauma { get; set; } = 0;
    public int PassiveDefense { get; set; } = 1;
    // For details like this enemy armor take -1 for each strike
    public string EnemySpec { get; set; } = string.Empty;
    public List<NPCArtifact> NPCArtifacts { get; set; } = [];
    public List<NPCAttribute> NPCAttributes { get; set; } = [];
    public List<NPCBurn> NPCBurns { get; set; } = [];
    public List<NPCSkill> NPCSkills { get; set; } = [];
    public List<NPCPotential> NPCPotentials { get; set; } = [];
    public List<NPCProtection> NPCProtections { get; set; } = [];
    public List<NPCEquipment> NPCEquipments { get; set; } = [];
    public List<NPCWeapon> NPCWeapons { get; set; } = [];
}
