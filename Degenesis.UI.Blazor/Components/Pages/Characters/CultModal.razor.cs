using Degenesis.Shared.DTOs.Characters;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Characters;


public partial class CultModal
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;

    [Parameter] public CultDto Cult { get; set; } = new();
    [Parameter] public List<SkillDto> Skills { get; set; } = new();
    private HttpClient _client = new();

    protected override async Task OnInitializedAsync()
    {
        _client = await HttpClientService.GetClientAsync();
    }

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
        {
            var result = await _client.PostAsJsonAsync("/cults", Cult);
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
            var result = await _client.PutAsJsonAsync($"/cults", Cult);
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
