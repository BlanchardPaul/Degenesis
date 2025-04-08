using Degenesis.Shared.DTOs.Vehicles;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Vehicles;

public partial class VehicleTypeList
{
    private List<VehicleTypeDto>? vehicleTypes;
    private HttpClient _client = new();

    protected override async Task OnInitializedAsync()
    {
        _client = await HttpClientService.GetClientAsync();
        await LoadVehicleTypes();
    }

    private async Task LoadVehicleTypes()
    {
        vehicleTypes = await _client.GetFromJsonAsync<List<VehicleTypeDto>>("/vehicle-types") ?? [];
    }

    private async Task ShowCreateDialog()
    {
        var parameters = new DialogParameters
            {
                { "VehicleType", new VehicleTypeDto() }
            };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, BackdropClick = false };

        var dialog = await DialogService.ShowAsync<VehicleTypeModal>("Create Vehicle Type", parameters, options);
        var result = await dialog.Result;

        if (result is not null && !result.Canceled)
        {
            await LoadVehicleTypes();
        }
    }

    private async Task ShowEditDialog(Guid vehicleTypeId)
    {
        var vehicleType = vehicleTypes?.FirstOrDefault(v => v.Id == vehicleTypeId);
        if (vehicleType != null)
        {
            var parameters = new DialogParameters
                {
                    { "VehicleType", vehicleType }
                };

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, BackdropClick = false };

            var dialog = await DialogService.ShowAsync<VehicleTypeModal>("Edit Vehicle Type", parameters, options);
            var result = await dialog.Result;

            if (result is not null && !result.Canceled)
            {
                await LoadVehicleTypes();
            }
        }
    }

    private async Task DeleteVehicleType(Guid vehicleTypeId)
    {
        var result = await _client.DeleteAsync($"/vehicle-types/{vehicleTypeId}");
        if (!result.IsSuccessStatusCode)
            Snackbar.Add("Error during deletion");
        else
            Snackbar.Add("Deleted");
        await LoadVehicleTypes();
    }
}