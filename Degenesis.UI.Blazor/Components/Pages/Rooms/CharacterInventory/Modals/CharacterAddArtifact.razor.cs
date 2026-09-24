using Degenesis.Shared.DTOs._Artifacts;
using Degenesis.Shared.DTOs.Characters.CRUD.Inventory;
using Degenesis.Shared.DTOs.Characters.Display;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Rooms.CharacterInventory.Modals;

public partial class CharacterAddArtifact
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;
    [Parameter] public CharacterDisplayDto Character { get; set; } = new();
    public List<ArtifactDto> Artifacts { get; set; } = [];

    private HttpClient _client = new();
    private bool ArtifactsLoaded;

    protected override async Task OnInitializedAsync()
    {
        _client = await HttpClientService.GetClientAsync();
        Artifacts = await _client.GetFromJsonAsync<List<ArtifactDto>>("/artifacts") ?? [];
        ArtifactsLoaded = true;
    }

    private async Task AddCharacterArtifact(Guid artifactId)
    {
        if (artifactId == Guid.Empty)
        {
            Snackbar.Add("Please select an artifact first.", Severity.Warning);
            return;
        }

        var result = await _client.PostAsJsonAsync($"/character-artifacts/", new CharacterArtifactCreateDto {Id = Guid.NewGuid(), CharacterId = Character.Id, ArtifactId = artifactId });
        if (!result.IsSuccessStatusCode)
            Snackbar.Add("Error while adding character artifact", Severity.Error);
        else
            Snackbar.Add("Character artifact added successfully.", Severity.Success);
        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel() => MudDialog.Cancel();
}
