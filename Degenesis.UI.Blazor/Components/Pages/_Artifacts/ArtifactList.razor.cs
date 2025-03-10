using Degenesis.Shared.DTOs._Artifacts;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages._Artifacts;

public partial class ArtifactList
{
    [Inject] private IDialogService DialogService { get; set; } = default!;
    private List<ArtifactDto>? artifacts;

    protected override async Task OnInitializedAsync()
    {
        await LoadArtifacts();
    }
    private async Task LoadArtifacts()
    {
        artifacts = await ArtifactService.GetArtifactsAsync();
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
        var artifact = artifacts?.FirstOrDefault(a => a.Id == artifactId);
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
        await ArtifactService.DeleteArtifactAsync(artifactId);
        artifacts = await ArtifactService.GetArtifactsAsync();
    }
}

