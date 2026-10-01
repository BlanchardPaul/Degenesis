using Degenesis.Shared.DTOs.Characters.CRUD;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Characters.Dashboard;


public partial class CultModal
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;

    [Parameter] public CultDto Cult { get; set; } = new();
    [Parameter] public List<SkillDto> Skills { get; set; } = [];

    private List<Guid> SelectedBonusSkillIds { get; set; } = [];

    protected override void OnParametersSet()
    {
        Cult.BonusSkills ??= [];
        SelectedBonusSkillIds = [.. Cult.BonusSkills.Select(bs => bs.Id)];
    }

    private Task OnBonusSkillsChanged(IEnumerable<Guid> selectedValues)
    {
        SelectedBonusSkillIds = [.. selectedValues];
        Cult.BonusSkills = [.. Skills.Where(s => SelectedBonusSkillIds.Contains(s.Id))];
        return Task.CompletedTask;
    }

    private async Task SaveCult()
    {
        if (Cult.Id == Guid.Empty)
        {
            var result = await Client!.PostAsJsonAsync("/cults", Cult);
            if (!result.IsSuccessStatusCode)
                Snackbar.Add("Error during creation", Severity.Error);
            else
            {
                Snackbar.Add("Created", Severity.Success);
                MudDialog.Close(DialogResult.Ok(true));
            }
        }

        else
        {
            var result = await Client!.PutAsJsonAsync($"/cults", Cult);
            if (!result.IsSuccessStatusCode)
                Snackbar.Add("Error during edition", Severity.Error);
            else
            {
                Snackbar.Add("Edited", Severity.Success);
                MudDialog.Close(DialogResult.Ok(true));
            }
        }
        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel() => MudDialog.Cancel();
}
