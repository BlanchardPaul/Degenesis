using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs._Artifacts;
using Degenesis.UI.Blazor.Extensions;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages._Artifacts;

public partial class ArtifactList : AuthenticatedComponentBase
{
    private List<ArtifactDto>? Artifacts;
    private string SearchString = "";

    protected override async Task OnAuthenticatedInitializedAsync()
    {
        await LoadArtifacts();
    }

    private async Task LoadArtifacts()
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
        var response = await Client!.DeleteAsync($"/artifacts/{artifactId}");
        if(!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result<object>>();
            Snackbar.Add(result?.Error ?? "Unknown error", Severity.Error);
            return;
        }
        else
            Snackbar.Add("Deleted", Severity.Success);

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