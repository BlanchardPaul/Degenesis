using Degenesis.Shared.DTOs.Characters;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Characters;

public partial class PotentialList
{
    private List<PotentialDto>? potentials;
    private List<CultDto> cults = new();

    protected override async Task OnInitializedAsync()
    {
        await LoadPotentials();
    }

    private async Task LoadPotentials()
    {
        potentials = [.. (await PotentialService.GetPotentialsAsync())];
        cults = [.. (await CultService.GetCultsAsync())];
    }

    private async Task ShowCreateDialog()
    {
        var parameters = new DialogParameters
        {
            { "Potential", new PotentialDto() },
            { "Cults", cults }
        };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, BackdropClick = false };

        var dialog = await DialogService.ShowAsync<PotentialModal>("Create Potential", parameters, options);
        var result = await dialog.Result;

        if (result is not null && !result.Canceled)
        {
            await LoadPotentials();
        }
    }

    private async Task ShowEditDialog(Guid potentialId)
    {
        var potential = potentials?.FirstOrDefault(p => p.Id == potentialId);
        if (potential != null)
        {
            var parameters = new DialogParameters
            {
                { "Potential", potential },
                { "Cults", cults }
            };

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, BackdropClick = false };

            var dialog = await DialogService.ShowAsync<PotentialModal>("Edit Potential", parameters, options);
            var result = await dialog.Result;

            if (result is not null && !result.Canceled)
            {
                await LoadPotentials();
            }
        }
    }

    private async Task DeletePotential(Guid potentialId)
    {
        await PotentialService.DeletePotentialAsync(potentialId);
        await LoadPotentials();
    }
}
