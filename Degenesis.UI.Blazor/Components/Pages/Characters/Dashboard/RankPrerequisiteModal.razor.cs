using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Characters.CRUD;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Characters.Dashboard;

public partial class RankPrerequisiteModal
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;

    [Parameter] public RankPrerequisiteDto RankPrerequisite { get; set; } = new();
    [Parameter] public List<AttributeDto> Attributes { get; set; } = [];
    [Parameter] public List<SkillDto> Skills { get; set; } = [];
    [Parameter] public List<BackgroundDto> Backgrounds { get; set; } = [];

    protected override void OnParametersSet()
    {
        if (RankPrerequisite.AttributeRequiredId == Guid.Empty && Attributes.Count != 0)
        {
            RankPrerequisite.AttributeRequiredId = Attributes.First().Id;
        }
    }

    private async Task SaveRankPrerequisite()
    {
        if (RankPrerequisite.IsBackgroundPrerequisite)
        {
            RankPrerequisite.AttributeRequiredId = null;
            RankPrerequisite.SkillRequiredId = null;
            RankPrerequisite.SumRequired = null;
        }
        else
        {
            RankPrerequisite.BackgroundRequired = null;
            RankPrerequisite.BackgroundLevelRequired = null;
        }

        HttpResponseMessage response;
        if (RankPrerequisite.Id == Guid.Empty)
            response = await Client!.PostAsJsonAsync("/rank-prerequisites", RankPrerequisite);
        else
            response = await Client!.PutAsJsonAsync("/rank-prerequisites", RankPrerequisite);

        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result<object>>();
            Snackbar.Add(result?.Error ?? "Unknown error", Severity.Error);
            return;
        }

        Snackbar.Add(RankPrerequisite.Id == Guid.Empty ? "Created" : "Edited", Severity.Success);

        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel() => MudDialog.Cancel();
}
