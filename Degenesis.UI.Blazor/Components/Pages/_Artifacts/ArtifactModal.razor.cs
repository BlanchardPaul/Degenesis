using Degenesis.Shared.DTOs._Artifacts;
using Degenesis.UI.Service.Features;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.ComponentModel.DataAnnotations;

namespace Degenesis.UI.Blazor.Components.Pages._Artifacts;

public partial class ArtifactModal
{
    [Inject] private ArtifactService ArtifactService { get; set; } = default!;
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; }
    [Parameter] public ArtifactDto Artifact { get; set; } = new();

    private async Task SaveArtifact()
    {
        if (Artifact.Id == Guid.Empty)
            await ArtifactService.CreateArtifactAsync(Artifact);
        else
            await ArtifactService.UpdateArtifactAsync(Artifact);

        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel() => MudDialog.Cancel();
}