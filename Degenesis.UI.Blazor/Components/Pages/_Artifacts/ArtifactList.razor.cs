using Degenesis.Shared.DTOs._Artifacts;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages._Artifacts;

public partial class ArtifactList
{
    private List<ArtifactDto>? Artifacts;
    private HttpClient _client = new();
    private string SearchString = "";

    protected override async Task OnInitializedAsync()
    {
        _client = await HttpClientService.GetClientAsync();
        await LoadArtifacts();
    }

    private async Task LoadArtifacts()
    {
        Artifacts = await _client.GetFromJsonAsync<List<ArtifactDto>>("/artifacts") ?? [];
    }

    private async Task ShowCreateDialog()
    {
        var parameters = new DialogParameters { { "Artifact", new ArtifactDto() } };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, BackdropClick = false };

        var dialog = await DialogService.ShowAsync<ArtifactModal>("Create Artifact", parameters, options);
        var result = await dialog.Result;

        if (result is not null && !result.Canceled)
        {
            await LoadArtifacts();
        }
    }

    private async Task ShowEditDialog(Guid artifactId)
    {
        var artifact = Artifacts?.FirstOrDefault(a => a.Id == artifactId);
        if (artifact != null)
        {
            var parameters = new DialogParameters { { "Artifact", artifact } };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, BackdropClick = false };

            var dialog = await DialogService.ShowAsync<ArtifactModal>("Edit Artifact", parameters, options);
            var result = await dialog.Result;

            if (result is not null && !result.Canceled)
            {
                await LoadArtifacts();
            }
        }
    }

    private async Task DeleteArtifact(Guid artifactId)
    {
        var result = await _client.DeleteAsync($"/artifacts/{artifactId}");
        if (!result.IsSuccessStatusCode)
            Snackbar.Add("Error during deletion");
        else
            Snackbar.Add("Deleted");

        await LoadArtifacts();
    }

    private bool FilterFunc(ArtifactDto artifact)
    {
        if (string.IsNullOrWhiteSpace(SearchString))
            return true;
        if (artifact.Name.Contains(SearchString, StringComparison.OrdinalIgnoreCase))
            return true;
        return false;
    }
}

