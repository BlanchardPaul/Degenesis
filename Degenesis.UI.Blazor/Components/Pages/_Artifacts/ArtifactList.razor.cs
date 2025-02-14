using Degenesis.Shared.DTOs._Artifacts;

namespace Degenesis.UI.Blazor.Components.Pages._Artifacts;

public partial class ArtifactList
{
    private List<ArtifactDto>? artifacts;
    private ArtifactDto currentArtifact = new();
    private bool isModalVisible = false;
    private string modalTitle = "Create Artifact";

    protected override async Task OnInitializedAsync()
    {
        artifacts = await ArtifactService.GetArtifactsAsync();
    }

    private void ShowCreateDialog()
    {
        Console.WriteLine("ShowCreateDialog appelé");
        modalTitle = "Create Artifact";
        currentArtifact = new ArtifactDto();
        isModalVisible = true;
    }

    private void ShowEditDialog(Guid artifactId)
    {
        Console.WriteLine("ShowEditDialog appelé");
        var artifact = artifacts?.FirstOrDefault(a => a.Id == artifactId);
        if (artifact != null)
        {
            modalTitle = "Edit Artifact";
            currentArtifact = new ArtifactDto
            {
                Id = artifact.Id,
                Name = artifact.Name,
                Description = artifact.Description,
                EnergyStorage = artifact.EnergyStorage
            };
            isModalVisible = true;
        }
    }

    private async Task SaveArtifact()
    {
        if (currentArtifact.Id == Guid.Empty)
            await ArtifactService.CreateArtifactAsync(currentArtifact);
        else
            await ArtifactService.UpdateArtifactAsync(currentArtifact);

        artifacts = await ArtifactService.GetArtifactsAsync();
        await CloseModal();
    }

    private async Task DeleteArtifact(Guid artifactId)
    {
        await ArtifactService.DeleteArtifactAsync(artifactId);
        artifacts = await ArtifactService.GetArtifactsAsync();
    }

    private async Task CloseModal()
    {
        Console.WriteLine("Fermeture modale");
        await InvokeAsync(() =>
        {
            isModalVisible = false;
        });
    }
}

