using Degenesis.Shared.DTOs.Characters;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Characters;

public partial class PotentialList
{
    private List<PotentialDto>? potentials;
    private List<CultDto> cults = new();
    private HttpClient _client = new();

    protected override async Task OnInitializedAsync()
    {
        _client = await HttpClientService.GetClientAsync();
        await LoadPotentials();
    }

    private async Task LoadPotentials()
    {
        potentials = await _client.GetFromJsonAsync<List<PotentialDto>>("/potentials") ?? [];
        cults =await _client.GetFromJsonAsync<List<CultDto>>("/cults") ?? [];
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
        var result = await _client.DeleteAsync($"/potentials/{potentialId}");
        if (!result.IsSuccessStatusCode)
            Snackbar.Add("Error during deletion");
        else
            Snackbar.Add("Deleted");
        await LoadPotentials();
    }
}
