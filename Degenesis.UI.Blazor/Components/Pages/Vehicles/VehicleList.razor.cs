using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Characters.CRUD;
using Degenesis.Shared.DTOs.Vehicles;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Vehicles;

public partial class VehicleList
{
    private List<VehicleDto>? Vehicles;
    private List<VehicleTypeDto> VehicleTypes = [];
    private List<CultDto> Cults = [];
    private string SearchString = "";

    protected override async Task OnAuthenticatedInitializedAsync()
    {
        await LoadVehicles();
    }

    private async Task LoadVehicles()
    {
        var vehicleResult = await Client!.GetFromJsonAsync<Result<List<VehicleDto>>>("/vehicles") ?? new Result<List<VehicleDto>> { IsError = true, Error = "Unknown error" };
        if (vehicleResult.IsError)
        {
            Snackbar.Add($"Error loading vehicles: {vehicleResult.Error}", Severity.Error);
            Vehicles = [];
        }
        else
            Vehicles = vehicleResult.Value ?? [];

        var vehicleTypeResult = await Client!.GetFromJsonAsync<Result<List<VehicleTypeDto>>>("/vehicle-types") ?? new Result<List<VehicleTypeDto>> { IsError = true , Error = "Unknown error" };
        if (vehicleTypeResult.IsError)
        {
            Snackbar.Add($"Error loading vehicle types: {vehicleTypeResult.Error}", Severity.Error);
            VehicleTypes = [];
        }
        else
            VehicleTypes = vehicleTypeResult.Value ?? [];

        var cultResult = await Client!.GetFromJsonAsync<Result<List<CultDto>>>("/cults") ?? new Result<List<CultDto>> { IsError = true, Error = "Unknown error" };
        if (cultResult.IsError)
        {
            Snackbar.Add($"Error loading cults: {cultResult.Error}", Severity.Error);
            Cults = [];
        }
        else
            Cults = cultResult.Value ?? [];
    }

    private async Task ShowCreateDialog()
    {
        var parameters = new DialogParameters
            {
                { "Vehicle", new VehicleDto() },
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
        var vehicle = Vehicles?.FirstOrDefault(v => v.Id == vehicleId);
        if (vehicle != null)
        {
            var parameters = new DialogParameters
                {
                    { "Vehicle", vehicle },
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
        var response = await Client!.DeleteAsync($"/vehicles/{vehicleId}");
        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result<object>>();
            Snackbar.Add(result?.Error ?? "Unknown error", Severity.Error);
        }
        else
            Snackbar.Add("Deleted", Severity.Success);
        await LoadVehicles();
    }

    private bool FilterFunc(VehicleDto vehicle)
    {
        if (string.IsNullOrWhiteSpace(SearchString))
            return true;
        if (vehicle.Name.Contains(SearchString, StringComparison.OrdinalIgnoreCase))
            return true;
        return false;
    }
}