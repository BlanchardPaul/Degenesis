using Degenesis.Shared.DTOs.Characters.CRUD;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Characters;

public partial class CharacterPotentialStep
{
    [Parameter] public List<PotentialDto> Potentials { get; set; } = [];
    [Parameter] public CharacterCreateDto Character { get; set; } = new();

    private MudForm _formStepPotentials = default!;
    private Guid SelectedPotentialId { get; set; } = Guid.Empty;

    public async Task<bool> ValidateFormAsync()
    {
        await _formStepPotentials.Validate();

        if (SelectedPotentialId != Guid.Empty)
        {
            // overwrite with exactly one
            Character.Potentials = [new CharacterPotentialDto{
                CharacterId = Guid.Empty,
                PotentialId = SelectedPotentialId,
                Level = 1
            }];
        }

        return true;
    }

    private bool IsPotentialDisabled(PotentialDto potential)
    {
        foreach (var prereq in potential.Prerequisites ?? Enumerable.Empty<PotentialPrerequisiteDto>())
        {
            if (prereq.IsBackgroundPrerequisite)
            {
                var bg = Character.Backgrounds.FirstOrDefault(b => b.BackgroundId == prereq.BackgroundRequiredId!.Value);
                if (bg == null || bg.Level < (prereq.BackgroundLevelRequired ?? 0))
                    return true;
            }

            else if (prereq.IsRankPrerequisite)
            {
                if (Character.RankId != prereq.RankRequiredId)
                    return true;
            }

            else
            {
                int attrLevel = 0;
                int skillLevel = 0;

                if (prereq.AttributeRequiredId.HasValue)
                    attrLevel = Character.Attributes.FirstOrDefault(a => a.AttributeId == prereq.AttributeRequiredId)?.Level ?? 0;

                if (prereq.SkillRequiredId.HasValue)
                    skillLevel = Character.Skills.FirstOrDefault(s => s.SkillId == prereq.SkillRequiredId)?.Level ?? 0;

                int sum = attrLevel + skillLevel;

                if (!prereq.SumRequired.HasValue || sum < prereq.SumRequired.Value)
                    return true;
            }
        }

        return false;
    }

    private static string GetPotentialPrerequisiteLabel(PotentialPrerequisiteDto prerequisite)
    {
        if (prerequisite == null)
            return "Unknown";

        if (prerequisite.IsBackgroundPrerequisite && prerequisite.BackgroundRequired != null)
        {
            return $"{prerequisite.BackgroundRequired.Name} >= {prerequisite.BackgroundLevelRequired}";
        }

        if (prerequisite.IsRankPrerequisite && prerequisite.RankRequired != null)
        {
            return $"Rank: {prerequisite.RankRequired.Name}";
        }

        string attributePart = prerequisite.AttributeRequired?.Name ?? "";
        string skillPart = prerequisite.SkillRequired != null ? $" + {prerequisite.SkillRequired.Name}" : "";
        string sumPart = prerequisite.SumRequired.HasValue ? $" >= {prerequisite.SumRequired}" : "";

        return $"{attributePart}{skillPart}{sumPart}".Trim();
    }

}