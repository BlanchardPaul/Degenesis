using Degenesis.Shared.DTOs;
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
        var result = await Client!.GetFromJsonAsync<Result<List<ArtifactDto>>>("/artifacts") ?? new Result<List<ArtifactDto>> { IsError = true, Error = "Unknown error" };
        if (result.IsError)
        {
            Snackbar.Add("Error loading artifacts: " + result.Error);
            Artifacts = [];
        }
        else
        {
            Artifacts = result.Value;
        }
    }

    private async Task AddCharacterArtifact(Guid artifactId)
    {
        if (artifactId == Guid.Empty)
        {
            Snackbar.Add("Please select an artifact first.", Severity.Warning);
            return;
        }

        var response = await Client!.PostAsJsonAsync($"/character-artifacts/", new CharacterArtifactCreateDto {Id = Guid.NewGuid(), CharacterId = Character.Id, ArtifactId = artifactId });
        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result<object>>();
            Snackbar.Add(result?.Error ?? "Unknown error", Severity.Error);
            return;
        }
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
