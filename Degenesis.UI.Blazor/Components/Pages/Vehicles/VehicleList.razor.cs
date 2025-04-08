using Degenesis.Shared.DTOs.Vehicles;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Vehicles;

public partial class VehicleList
{
    private List<VehicleDto>? vehicles;
    private List<VehicleTypeDto> vehicleTypes = [];
    private HttpClient _client = new();

    protected override async Task OnInitializedAsync()
    {
        _client = await HttpClientService.GetClientAsync();
        await LoadVehicles();
    }

    private async Task LoadVehicles()
    {
        vehicles = await _client.GetFromJsonAsync<List<VehicleDto>>("/vehicles") ?? [];
        vehicleTypes = await _client.GetFromJsonAsync<List<VehicleTypeDto>>("/vehicle-types") ?? [];
    }

    private async Task ShowCreateDialog()
    {
        var parameters = new DialogParameters
            {
                { "Vehicle", new VehicleDto() },
                { "VehicleTypes", vehicleTypes }
            };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, BackdropClick = false };

        var dialog = await DialogService.ShowAsync<VehicleModal>("Create Vehicle", parameters, options);
        var result = await dialog.Result;

        if (result is not null && !result.Canceled)
        {
            await LoadVehicles();
        }
    }

    private async Task ShowEditDialog(Guid vehicleId)
    {
        var vehicle = vehicles?.FirstOrDefault(v => v.Id == vehicleId);
        if (vehicle != null)
        {
            var parameters = new DialogParameters
                {
                    { "Vehicle", vehicle },
                    { "VehicleTypes", vehicleTypes }
                };

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, BackdropClick = false };

            var dialog = await DialogService.ShowAsync<VehicleModal>("Edit Vehicle", parameters, options);
            var result = await dialog.Result;

            if (result is not null && !result.Canceled)
            {
                await LoadVehicles();
            }
        }
    }

    private async Task DeleteVehicle(Guid vehicleId)
    {
        var result = await _client.DeleteAsync($"/vehicles/{vehicleId}");
        if (!result.IsSuccessStatusCode)
            Snackbar.Add("Error during deletion");
        else
            Snackbar.Add("Deleted");
        await LoadVehicles();
    }
}