using Domain._Artifacts;
using Domain.Burns;
using Domain.Equipments;
using Domain.Protections;
using Domain.Weapons;

namespace Domain.NPCs;

public class BaseNPC
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Initiative { get; set; } = string.Empty;
    public string Specialty {  get; set; } = string.Empty;
    public string Attack { get; set; } = string.Empty;
    public string Defense { get; set; } = string.Empty;
    public string Movement { get; set; } = string.Empty;
    public string Armor { get; set; } = string.Empty;
    public string Equipment { get; set; } = string.Empty;
    public string Condition { get; set; } = string.Empty;
    public string Notes {  get; set; } = string.Empty;
    public List<BaseNPCAttribute> NPCAttributes { get; set; } = [];
    public List<BaseNPCSkill> NPCSkills { get; set; } = [];
}
