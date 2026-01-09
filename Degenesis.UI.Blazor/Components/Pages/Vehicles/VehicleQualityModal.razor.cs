using Degenesis.Shared.DTOs.Vehicles;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Vehicles;

public partial class VehicleQualityModal
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;

    [Parameter] public VehicleQualityDto VehicleQuality { get; set; } = new();
    private HttpClient _client = new();

    protected override async Task OnInitializedAsync()
    {
        _client = await HttpClientService.GetClientAsync();
    }

    private async Task SaveVehicleQuality()
    {
        if (VehicleQuality.Id == Guid.Empty)
        {
            var result = await _client.PostAsJsonAsync($"/vehicle-qualities", VehicleQuality);
            if (!result.IsSuccessStatusCode)
                Snackbar.Add("Error during creation", Severity.Error);
            else
            {
                Snackbar.Add("Created", Severity.Success);
                MudDialog.Close(DialogResult.Ok(true));
            }
        }
        else
        {
            var result = await _client.PutAsJsonAsync($"/vehicle-qualities", VehicleQuality);
            if (!result.IsSuccessStatusCode)
                Snackbar.Add("Error during edition", Severity.Error);
            else
            {
                Snackbar.Add("Edited", Severity.Success);
                MudDialog.Close(DialogResult.Ok(true));
            }
        }
        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel() => MudDialog.Cancel();
}
