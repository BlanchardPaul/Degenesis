using Degenesis.Shared.DTOs.Characters.CRUD;
using Degenesis.Shared.DTOs.Vehicles;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Vehicles;

public partial class VehicleList
{
    private List<VehicleDto>? Vehicle;
    private List<VehicleQualityDto> VehicleQualities = [];
    private List<VehicleTypeDto> VehicleTypes = [];
    private List<CultDto> Cults = [];
    private HttpClient _client = new();

    protected override async Task OnInitializedAsync()
    {
        _client = await HttpClientService.GetClientAsync();
        await LoadVehicles();
    }

    private async Task LoadVehicles()
    {
        Vehicle = await _client.GetFromJsonAsync<List<VehicleDto>>("/vehicles") ?? [];
        VehicleQualities = await _client.GetFromJsonAsync<List<VehicleQualityDto>>("/vehicle-qualities") ?? [];
        VehicleTypes = await _client.GetFromJsonAsync<List<VehicleTypeDto>>("/vehicle-types") ?? [];
        Cults = await _client.GetFromJsonAsync<List<CultDto>>("/cults") ?? [];
    }

    private async Task ShowCreateDialog()
    {
        var parameters = new DialogParameters
            {
                { "Vehicle", new VehicleDto() },
                { "VehicleQualities", VehicleQualities },
                { "VehicleTypes", VehicleTypes },
                { "Cults", Cults }
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
        var vehicle = Vehicle?.FirstOrDefault(v => v.Id == vehicleId);
        if (vehicle != null)
        {
            var parameters = new DialogParameters
                {
                    { "Vehicle", vehicle },
                    { "VehicleQualities", VehicleQualities },
                    { "VehicleTypes", VehicleTypes },
                    { "Cults", Cults }
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