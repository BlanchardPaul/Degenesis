using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Characters.CRUD;
using Degenesis.Shared.DTOs.Characters.Display;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Rooms.CharacterSheetModals;

public partial class CharacterEditRankModal
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;
    [Parameter] public CharacterDisplayDto Character { get; set; } = new();
    private Guid SelectedRankId;
    public List<RankDto> Ranks { get; set; } = [];

    protected override async Task OnAuthenticatedInitializedAsync()
    {
        Ranks = await Client!.GetFromJsonAsync<List<RankDto>>("/ranks") ?? [];
        Ranks = [.. Ranks.Where(r => r.CultId == Character.Cult.Id)];
    }

    private Task OnRankSelected(Guid rankId)
    {
        SelectedRankId = rankId;
        StateHasChanged();
        return Task.CompletedTask;
    }


    private async Task ConfirmSelection()
    {
        if (SelectedRankId == Guid.Empty)
        {
            Snackbar.Add("Please select a rank first.", Severity.Warning);
            return;
        }

        var response = await Client!.PutAsJsonAsync($"/characters/rank/", new CharacterGuidValueEditDto { Id = Character.Id, Value = SelectedRankId });

        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result<object>>();
            Snackbar.Add(result?.Error ?? "Unknown error", Severity.Error);
        }
        else
            Snackbar.Add("Rank updated.", Severity.Success);

        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel() => MudDialog.Cancel();
}
