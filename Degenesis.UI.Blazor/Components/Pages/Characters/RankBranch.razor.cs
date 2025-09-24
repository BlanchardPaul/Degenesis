using Degenesis.Shared.DTOs.Characters;
using Microsoft.AspNetCore.Components;

namespace Degenesis.UI.Blazor.Components.Pages.Characters;

public partial class RankBranch
{
    [Parameter] public RankDto Rank { get; set; } = default!;
    [Parameter] public List<RankDto> AllRanks { get; set; } = [];
    [Parameter] public Guid SelectedRankId { get; set; }
    [Parameter] public EventCallback<Guid> OnRankSelected { get; set; }
    [Parameter] public CharacterCreateDto Character { get; set; } = default!;

    private List<RankDto> Children =>
        AllRanks.Where(r => r.ParentRankId == Rank.Id).ToList();

    private bool IsRankDisabled(RankDto rank)
    {
        if (rank.ParentRankId != null)
        {
            var parent = AllRanks.FirstOrDefault(r => r.Id == rank.ParentRankId);
            if (parent != null && IsRankDisabled(parent))
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
                int attrLevel = Character.Attributes.FirstOrDefault(a => a.AttributeId == prereq.AttributeRequiredId)?.Level ?? 0;
                int skillLevel = Character.Skills.FirstOrDefault(s => s.SkillId == prereq.SkillRequiredId)?.Level ?? 0;
                int sum = attrLevel + skillLevel;

                if (prereq.SumRequired.HasValue)
                {
                    if (sum < prereq.SumRequired.Value) return true;
                }
                else
                {
                    if (prereq.SkillRequiredId.HasValue && sum < 1) return true;
                    if (!prereq.SkillRequiredId.HasValue && attrLevel < 1) return true;
                }
            }
        }

        return false;
    }
}