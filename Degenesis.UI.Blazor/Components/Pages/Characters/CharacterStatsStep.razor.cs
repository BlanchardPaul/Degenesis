using Degenesis.Shared.DTOs.Characters;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Characters;
public partial class CharacterStatsStep
{
    [Parameter] public List<AttributeDto> Attributes { get; set; } = [];
    [Parameter] public List<SkillDto> Skills { get; set; } = [];
    [Parameter] public CultureDto SelectedCulture { get; set; } = new();
    [Parameter] public CultDto SelectedCult { get; set; } = new();
    [Parameter] public ConceptDto SelectedConcept { get; set; } = new();

    [Parameter] public List<CharacterAttributeDto> CharacterAttributes { get; set; } = [];
    [Parameter] public List<CharacterSkillDto> CharacterSkills { get; set; } = [];

    public int MaxEgo;
    public int MaxSporeInfestation;
    public int MaxFleshWounds;
    public int MaxTrauma;

    private MudForm _formStep2 = default!;
    private const int MaxAttributePoints = 10;
    private const int MaxSkillPoints = 28;

    private Guid FocusSkillId => Skills.First(s => s.Name == "FOCUS").Id;
    private Guid PrimalSkillId => Skills.First(s => s.Name == "PRIMAL").Id;
    private Guid FaithSkillId => Skills.First(s => s.Name == "FAITH").Id;
    private Guid WillpowerSkillId => Skills.First(s => s.Name == "WILLPOWER").Id;
    private Guid ToughnessSkillId => Skills.First(s => s.Name == "TOUGHNESS").Id;
    private Guid IntellectAttributeId => Attributes.First(a => a.Name == "INTELLECT").Id;
    private Guid InstinctAttributeId => Attributes.First(a => a.Name == "INSTINCT").Id;
    private Guid PsycheAttributeId => Attributes.First(a => a.Name == "PSYCHE").Id;
    private Guid BodyAttributeId => Attributes.First(a => a.Name == "BODY").Id;

    protected override void OnParametersSet()
    {
        foreach (var attr in Attributes)
            if (!CharacterAttributes.Any(a => a.AttributeId == attr.Id))
                CharacterAttributes.Add(new CharacterAttributeDto { AttributeId = attr.Id, Level = 1 });

        foreach (var skill in Skills)
            if (!CharacterSkills.Any(s => s.SkillId == skill.Id))
                CharacterSkills.Add(new CharacterSkillDto { SkillId = skill.Id, Level = 0 });
    }

    private int GetAttributeLevel(Guid attributeId) =>
    CharacterAttributes.First(a => a.AttributeId == attributeId).Level;

    private void SetAttributeLevel(Guid attributeId, int value)
    {
        var attr = CharacterAttributes.First(a => a.AttributeId == attributeId);
        attr.Level = value;
    }

    private int GetSkillLevel(Guid skillId) =>
        CharacterSkills.First(s => s.SkillId == skillId).Level;

    private void SetSkillLevel(Guid skillId, int value)
    {
        var skill = CharacterSkills.First(s => s.SkillId == skillId);
        skill.Level = value;
    }

    private int GetTotalAttributesUsed() =>
        CharacterAttributes.Sum(a => a.Level - 1);

    private int GetTotalSkillsUsed() =>
        CharacterSkills.Sum(s => s.Level);

    private int BindAttributeValue(Guid attrId)
    {
        return GetAttributeLevel(attrId);
    }

    private void BindAttributeValueChanged(Guid attrId, int value)
    {
        SetAttributeLevel(attrId, value);
        ValidateTotalAttributes();
        ComputeCharacterDerivedStats();
    }

    private int BindSkillValue(Guid skillId)
    {
        return GetSkillLevel(skillId);
    }

    private void BindSkillValueChanged(Guid skillId, int value)
    {
        OnSkillValueChanged(skillId, value);
        ValidateTotalSkills();
        ComputeCharacterDerivedStats();
    }


    private int GetAttributeMax(Guid attributeId)
    {
        int max = 3;
        if (SelectedCulture.BonusAttributes.Any(a => a.Id == attributeId)) max++;
        if (SelectedConcept.BonusAttribute.Id == attributeId) max++;
        return max;
    }

    private int GetSkillMax(Guid skillId)
    {
        var max = 2;
        if (SelectedCulture.BonusSkills.Any(s => s.Id == skillId)) max++;
        if (SelectedCult.BonusSkills.Any(s => s.Id == skillId)) max++;
        if (SelectedConcept.BonusSkills.Any(s => s.Id == skillId)) max++;
        return max;
    }

    private void ValidateTotalAttributes()
    {
        var total = GetTotalAttributesUsed();
        if (total > MaxAttributePoints)
            Snackbar.Add($"You used too many points in attributes ({total}/{MaxAttributePoints})", Severity.Error);
    }

    private void ValidateTotalSkills()
    {
        var total = GetTotalSkillsUsed();
        if (total > MaxSkillPoints)
            Snackbar.Add($"You used too many points in skills ({total}/{MaxSkillPoints})", Severity.Error);
    }

    private bool IsAttributeDisabled(Guid attributeId)
    {
        var total = GetTotalAttributesUsed();
        var current = GetAttributeLevel(attributeId);
        return total >= MaxAttributePoints && current <= 1;
    }

    private void OnSkillValueChanged(Guid skillId, int newValue)
    {
        SetSkillLevel(skillId, newValue);

        if (skillId == FaithSkillId && newValue > 0)
            SetSkillLevel(WillpowerSkillId, 0);
        else if (skillId == WillpowerSkillId && newValue > 0)
            SetSkillLevel(FaithSkillId, 0);

        if (skillId == FocusSkillId && newValue > 0)
            SetSkillLevel(PrimalSkillId, 0);
        else if (skillId == PrimalSkillId && newValue > 0)
            SetSkillLevel(FocusSkillId, 0);
    }

    private void ComputeCharacterDerivedStats()
    {
        int GetAttribute(Guid id) => CharacterAttributes.FirstOrDefault(a => a.AttributeId == id)?.Level ?? 1;
        int GetSkill(Guid id) => CharacterSkills.FirstOrDefault(s => s.SkillId == id)?.Level ?? 0;

        int intellect = GetAttribute(IntellectAttributeId);
        int instinct = GetAttribute(InstinctAttributeId);
        int psyche = GetAttribute(PsycheAttributeId);
        int body = GetAttribute(BodyAttributeId);

        int focus = GetSkill(FocusSkillId);
        int primal = GetSkill(PrimalSkillId);
        int faith = GetSkill(FaithSkillId);
        int willpower = GetSkill(WillpowerSkillId);
        int toughness = GetSkill(ToughnessSkillId);

        MaxEgo = Math.Max((intellect + focus) * 2, (instinct + primal) * 2);
        MaxSporeInfestation = (psyche + Math.Max(faith, willpower)) * 2;
        MaxFleshWounds = (body + toughness) * 2;
        MaxTrauma = body + psyche;
    }

    public bool ValidateForm()
    {
        var totalAttr = GetTotalAttributesUsed();
        var totalSkills = GetTotalSkillsUsed();

        bool valid = true;

        if (totalAttr != MaxAttributePoints)
        {
            Snackbar.Add($"You must allocate exactly {MaxAttributePoints} attribute points (currently {totalAttr}).", Severity.Error);
            valid = false;
        }

        if (totalSkills != MaxSkillPoints)
        {
            Snackbar.Add($"You must allocate exactly {MaxSkillPoints} skill points (currently {totalSkills}).", Severity.Error);
            valid = false;
        }
        return valid;
    }

    public async Task<bool> ValidateFormAsync()
    {
        await _formStep2.Validate();
        return ValidateForm();
    }
}