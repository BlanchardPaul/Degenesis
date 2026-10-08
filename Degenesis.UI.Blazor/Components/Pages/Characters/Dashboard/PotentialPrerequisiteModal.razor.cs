using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Characters.CRUD;
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

        HttpResponseMessage response;
        if (PotentialPrerequisite.Id == Guid.Empty)
            response = await Client!.PostAsJsonAsync("/potential-prerequisites", PotentialPrerequisite);
        else
            response = await Client!.PutAsJsonAsync("/potential-prerequisites", PotentialPrerequisite);

        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result<object>>();
            Snackbar.Add(result?.Error ?? "Unknown error", Severity.Error);
            return;
        }

        Snackbar.Add(PotentialPrerequisite.Id == Guid.Empty ? "Created" : "Edited", Severity.Success);

        MudDialog.Close(DialogResult.Ok(true));
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
