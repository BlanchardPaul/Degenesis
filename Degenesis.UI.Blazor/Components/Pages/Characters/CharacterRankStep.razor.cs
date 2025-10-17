using Degenesis.Shared.DTOs.Characters.CRUD;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Characters;

public partial class CharacterRankStep
{
    [Parameter] public List<RankDto> Ranks { get; set; } = [];
    [Parameter] public CharacterCreateDto Character { get; set; } = new();

    private MudForm _formStepRank = default!;

    public bool ValidateForm()
    {
        if (Character.RankId == Guid.Empty)
        {
            Snackbar.Add("Please select a rank before continuing.", Severity.Error);
            return false;
        }
        return true;
    }

    public async Task<bool> ValidateFormAsync()
    {
        await _formStepRank.Validate();
        return ValidateForm();
    }
    private bool IsRankDisabled(RankDto rank)
    {
        if (rank.ParentRank != null && IsRankDisabled(rank.ParentRank))
        {
            return true;
        }

        foreach (var prereq in rank.Prerequisites ?? Enumerable.Empty<RankPrerequisiteDto>())
        {
            if (prereq.IsBackgroundPrerequisite)
            {
                var bg = Character.Backgrounds.FirstOrDefault(b => b.BackgroundId == prereq.BackgroundRequiredId);
                if (bg == null || bg.Level < prereq.BackgroundLevelRequired)
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

                if (prereq.SumRequired.HasValue)
                {
                    if (sum < prereq.SumRequired.Value)
                        return true;
                }
                else
                {
                    if (prereq.SkillRequiredId.HasValue)
                    {
                        if (sum < 1) return true;
                    }
                    else
                    {
                        if (attrLevel < 1) return true;
                    }
                }
            }
        }

        return false;
    }

}
