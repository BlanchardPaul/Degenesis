using Degenesis.Shared.DTOs.Characters.CRUD;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Rooms.CharactersSheetModals;

public partial class CharacterEditInfosModal
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;

    [Parameter] public CharacterBasicInfosEditDto CharacterInfos { get; set; } = new();

    private HttpClient _client = new();

    protected override async Task OnInitializedAsync()
    {
        _client = await HttpClientService.GetClientAsync();
    }

    private async Task SaveCharacterInfos()
    {
        var result = await _client.PutAsJsonAsync("/characters/basic-infos", CharacterInfos);

        if (!result.IsSuccessStatusCode)
        {
            Snackbar.Add("Error while saving character infos", Severity.Error);
            return;
        }

        Snackbar.Add("Character infos updated", Severity.Success);
        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel() => MudDialog.Cancel();
}