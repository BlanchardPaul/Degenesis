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
    public List<ArtifactDto>? Artifacts { get; set; }
    private string SearchString = "";

    protected override async Task OnAuthenticatedInitializedAsync()
    {
        Artifacts = await Client!.GetFromJsonAsync<List<ArtifactDto>>("/artifacts") ?? [];
    }

    private async Task AddCharacterArtifact(Guid artifactId)
    {
        if (artifactId == Guid.Empty)
        {
            Snackbar.Add("Please select an artifact first.", Severity.Warning);
            return;
        }

        var result = await Client!.PostAsJsonAsync($"/character-artifacts/", new CharacterArtifactCreateDto {Id = Guid.NewGuid(), CharacterId = Character.Id, ArtifactId = artifactId });
        if (!result.IsSuccessStatusCode)
            Snackbar.Add("Error while adding character artifact", Severity.Error);
        else
            Snackbar.Add("Character artifact added successfully.", Severity.Success);
        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel() => MudDialog.Cancel();

    private bool FilterFunc(ArtifactDto artifact)
    {
        if (string.IsNullOrWhiteSpace(SearchString))
            return true;
        if (artifact.Name.Contains(SearchString, StringComparison.OrdinalIgnoreCase))
            return true;
        return false;
    }
}
