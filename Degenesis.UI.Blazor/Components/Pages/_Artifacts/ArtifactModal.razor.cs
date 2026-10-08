using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs._Artifacts;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages._Artifacts;

public partial class ArtifactModal
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;
    [Parameter] public ArtifactDto Artifact { get; set; } = new();

    private async Task SaveArtifact()
    {
        HttpResponseMessage response;

        if (Artifact.Id == Guid.Empty)
            response = await Client!.PostAsJsonAsync("/artifacts", Artifact);
        else
            response = await Client!.PutAsJsonAsync("/artifacts", Artifact);

        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result<object>>();
            Snackbar.Add(result?.Error ?? "Unknown error", Severity.Error);
            return;
        }

        Snackbar.Add(Artifact.Id == Guid.Empty ? "Created" : "Edited", Severity.Success);
        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel() => MudDialog.Cancel();
}