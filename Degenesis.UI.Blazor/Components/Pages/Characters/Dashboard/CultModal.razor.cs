using Degenesis.Shared.DTOs;
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
        HttpResponseMessage response;
        if (Cult.Id == Guid.Empty)
            response = await Client!.PostAsJsonAsync("/cults", Cult);
        else
            response = await Client!.PutAsJsonAsync($"/cults", Cult);

        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result<object>>();
            Snackbar.Add(result?.Error ?? "Unknown error", Severity.Error);
            return;
        }

        Snackbar.Add(Cult.Id == Guid.Empty ? "Created" : "Edited", Severity.Success);

        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel() => MudDialog.Cancel();
}
