using Degenesis.Shared.DTOs.Characters.CRUD;
using Degenesis.Shared.DTOs.Characters.Display;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Rooms.CharactersSheetModals;

public partial class CharacterEditRankModal
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;
    [Parameter] public CharacterDisplayDto Character { get; set; } = new();
    private Guid SelectedRankId;
    public List<RankDto> Ranks { get; set; } = [];

    private HttpClient _client = new();

    protected override async Task OnInitializedAsync()
    {
        _client = await HttpClientService.GetClientAsync();
        Ranks = await _client.GetFromJsonAsync<List<RankDto>>("/ranks") ?? [];
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

        var result = await _client.PutAsJsonAsync($"/characters/rank/", new CharacterGuidValueEditDto { Id = Character.Id, Value = SelectedRankId });

        if (!result.IsSuccessStatusCode)
            Snackbar.Add("Error while updating rank", Severity.Error);
        else
            Snackbar.Add("Rank updated successfully.", Severity.Success);
        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel() => MudDialog.Cancel();
}
