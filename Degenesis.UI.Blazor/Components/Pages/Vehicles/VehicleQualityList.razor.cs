using Degenesis.Shared.DTOs.Vehicles;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Vehicles;

public partial class VehicleQualityList
{

    private List<VehicleQualityDto>? vehicleQualities;
    private HttpClient _client = new();

    protected override async Task OnInitializedAsync()
    {
        _client = await HttpClientService.GetClientAsync();
        await LoadVehicleQualities();
    }

    private async Task LoadVehicleQualities()
    {
        vehicleQualities = await _client.GetFromJsonAsync<List<VehicleQualityDto>>("/vehicle-qualities") ?? [];
    }

    private async Task ShowCreateDialog()
    {
        var parameters = new DialogParameters
            {
                { "VehicleQuality", new VehicleQualityDto() }
            };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, BackdropClick = false };

        var dialog = await DialogService.ShowAsync<VehicleQualityModal>("Create Vehicle Quality", parameters, options);
        var result = await dialog.Result;

        if (result is not null && !result.Canceled)
        {
            await LoadVehicleQualities();
        }
    }

    private async Task ShowEditDialog(Guid vehicleQualityId)
    {
        var vehicleQuality = vehicleQualities?.FirstOrDefault(wq => wq.Id == vehicleQualityId);
        if (vehicleQuality != null)
        {
            var parameters = new DialogParameters
                {
                    { "VehicleQuality", vehicleQuality }
                };

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, BackdropClick = false };

            var dialog = await DialogService.ShowAsync<VehicleQualityModal>("Edit Vehicle Quality", parameters, options);
            var result = await dialog.Result;

            if (result is not null && !result.Canceled)
            {
                await LoadVehicleQualities();
            }
        }
    }


    private async Task DeleteVehicleQuality(Guid vehicleQualityId)
    {
        var result = await _client.DeleteAsync($"/vehicle-qualities/{vehicleQualityId}");
        if (!result.IsSuccessStatusCode)
            Snackbar.Add("Error during deletion");
        else
            Snackbar.Add("Deleted");
        await LoadVehicleQualities();
    }
}
