using Degenesis.Shared.DTOs.Characters;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Characters.Dashboard;

public partial class PotentialPrerequisiteModal
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;

    [Parameter] public PotentialPrerequisiteDto PotentialPrerequisite { get; set; } = new();
    [Parameter] public List<AttributeDto> Attributes { get; set; } = [];
    [Parameter] public List<SkillDto> Skills { get; set; } = [];
    [Parameter] public List<BackgroundDto> Backgrounds { get; set; } = [];
    [Parameter] public List<RankDto> Ranks { get; set; } = [];

    private HttpClient _client = new();

    protected override async Task OnInitializedAsync()
    {
        _client = await HttpClientService.GetClientAsync();
    }

    private async Task SavePotentialPrerequisite()
    {
        // enforce flags and clear fields depending on type
        if (PotentialPrerequisite.IsBackgroundPrerequisite)
        {
            PotentialPrerequisite.IsRankPrerequisite = false;
            PotentialPrerequisite.AttributeRequiredId = null;
            PotentialPrerequisite.SkillRequiredId = null;
            PotentialPrerequisite.SumRequired = null;
            PotentialPrerequisite.RankRequiredId = null;
        }
        else if (PotentialPrerequisite.IsRankPrerequisite)
        {
            PotentialPrerequisite.IsBackgroundPrerequisite = false;
            PotentialPrerequisite.AttributeRequiredId = null;
            PotentialPrerequisite.SkillRequiredId = null;
            PotentialPrerequisite.SumRequired = null;
            PotentialPrerequisite.BackgroundRequiredId = null;
            PotentialPrerequisite.BackgroundLevelRequired = null;
        }
        else
        {
            PotentialPrerequisite.IsBackgroundPrerequisite = false;
            PotentialPrerequisite.IsRankPrerequisite = false;
            PotentialPrerequisite.BackgroundRequiredId = null;
            PotentialPrerequisite.BackgroundLevelRequired = null;
            PotentialPrerequisite.RankRequiredId = null;
        }

        if (PotentialPrerequisite.Id == Guid.Empty)
        {
            var result = await _client.PostAsJsonAsync("/potential-prerequisites", PotentialPrerequisite);
            if (!result.IsSuccessStatusCode)
            {
                Snackbar.Add("Error during creation", Severity.Error);
            }
            else
            {
                Snackbar.Add("Created", Severity.Success);
                MudDialog.Close(DialogResult.Ok(true));
            }
        }
        else
        {
            var result = await _client.PutAsJsonAsync("/potential-prerequisites", PotentialPrerequisite);
            if (!result.IsSuccessStatusCode)
            {
                Snackbar.Add("Error during edition", Severity.Error);
            }
            else
            {
                Snackbar.Add("Edited", Severity.Success);
                MudDialog.Close(DialogResult.Ok(true));
            }
        }
    }
    private void OnBackgroundCheckChanged(bool value)
    {
        PotentialPrerequisite.IsBackgroundPrerequisite = value;
        PotentialPrerequisite.IsRankPrerequisite = false;
    }

    private void OnRankCheckChanged(bool value)
    {
        PotentialPrerequisite.IsRankPrerequisite = value;
        PotentialPrerequisite.IsBackgroundPrerequisite = false;
    }
    private void Cancel() => MudDialog.Cancel();
}
