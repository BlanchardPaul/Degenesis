using Degenesis.Shared.DTOs.Characters;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Characters.Dashboard;

public partial class RankPrerequisiteModal
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;

    [Parameter] public RankPrerequisiteDto RankPrerequisite { get; set; } = new();
    [Parameter] public List<AttributeDto> Attributes { get; set; } = [];
    [Parameter] public List<SkillDto> Skills { get; set; } = [];
    private HttpClient _client = new();

    protected override async Task OnInitializedAsync()
    {
        _client = await HttpClientService.GetClientAsync();
    }

    protected override void OnParametersSet()
    {
        if (RankPrerequisite.AttributeRequiredId == Guid.Empty && Attributes.Count != 0)
        {
            RankPrerequisite.AttributeRequiredId = Attributes.First().Id;
        }
    }

    private async Task SaveRankPrerequisite()
    {
        if (RankPrerequisite.Id == Guid.Empty)
        {
            var result = await _client.PostAsJsonAsync("/rank-prerequisites", RankPrerequisite);
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
            var result = await _client.PutAsJsonAsync($"/rank-prerequisites", RankPrerequisite);
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
