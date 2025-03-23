using Degenesis.Shared.DTOs.Characters;
using Degenesis.UI.Service.Features.Characters;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Characters;

public partial class RankModal
{
    [Inject] private RankService RankService { get; set; } = default!;
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;

    [Parameter] public RankDto Rank { get; set; } = new();
    [Parameter] public List<CultDto> Cults { get; set; } = new();
    [Parameter] public List<RankPrerequisiteDto> RankPrerequisites { get; set; } = new();

    private HashSet<Guid> SelectedPrerequisiteIds { get; set; } = new();

    protected override void OnParametersSet()
    {
        Rank.Prerequisites ??= new List<RankPrerequisiteDto>();
        SelectedPrerequisiteIds = Rank.Prerequisites.Select(rp => rp.Id).ToHashSet();

        // Sélectionner le premier Cult si aucun n'est défini
        if (Rank.CultId == Guid.Empty && Cults.Any())
        {
            Rank.CultId = Cults.First().Id;
        }
    }

    private Task OnPrerequisitesChanged(IEnumerable<Guid> selectedValues)
    {
        SelectedPrerequisiteIds = selectedValues.ToHashSet();
        Rank.Prerequisites = RankPrerequisites.Where(rp => SelectedPrerequisiteIds.Contains(rp.Id)).ToList();
        return Task.CompletedTask;
    }

    private async Task SaveRank()
    {
        if (Rank.Id == Guid.Empty)
            await RankService.CreateRankAsync(Rank);
        else
            await RankService.UpdateRankAsync(Rank);

        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel() => MudDialog.Cancel();

    private string GetPrerequisiteLabel(Guid id)
    {
        var prerequisite = RankPrerequisites.FirstOrDefault(rp => rp.Id == id);
        if (prerequisite is null) return "Unknown Prerequisite";

        string skillPart = prerequisite.SkillRequired != null ? $" + {prerequisite.SkillRequired.Name}" : "";
        return $"{prerequisite.AttributeRequired.Name}{skillPart} = {prerequisite.SumRequired}";
    }
}
