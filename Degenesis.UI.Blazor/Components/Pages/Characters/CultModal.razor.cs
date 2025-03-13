using Degenesis.Shared.DTOs.Characters;
using Degenesis.UI.Service.Features.Characters;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Characters;

public partial class CultModal
{
    [Inject] private CultService CultService { get; set; } = default!;
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;

    [Parameter] public CultDto Cult { get; set; } = new();
    [Parameter] public List<SkillDto> Skills { get; set; } = new();

    // Liste temporaire pour MudSelect
    private HashSet<Guid> SelectedBonusSkillIds { get; set; } = new();

    protected override void OnParametersSet()
    {
        Cult.BonusSkills ??= new List<SkillDto>();

        // Assurer la préselection des valeurs existantes
        SelectedBonusSkillIds = Cult.BonusSkills.Select(bs => bs.Id).ToHashSet();
    }

    private Task OnBonusSkillsChanged(IEnumerable<Guid> selectedValues)
    {
        SelectedBonusSkillIds = new HashSet<Guid>(selectedValues);
        Cult.BonusSkills = Skills.Where(s => SelectedBonusSkillIds.ToList().Contains(s.Id)).ToList();
        return Task.CompletedTask;
    }

    private async Task SaveCult()
    {
        if (Cult.Id == Guid.Empty)
            await CultService.CreateCultAsync(Cult);
        else
            await CultService.UpdateCultAsync(Cult);

        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel() => MudDialog.Cancel();
}
