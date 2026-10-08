using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Characters.CRUD;
using Degenesis.Shared.DTOs.Vehicles;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Vehicles;

public partial class VehicleTypeList
{
    private List<VehicleTypeDto>? VehicleTypes;
    private string SearchString = "";

    protected override async Task OnAuthenticatedInitializedAsync()
    {
        await LoadVehicleTypes();
    }

    private async Task LoadVehicleTypes()
    {
        var vehicleResult = await Client!.GetFromJsonAsync<Result<List<VehicleTypeDto>>>("/vehicle-types") ?? new Result<List<VehicleTypeDto>> { IsError = true, Error = "Unknown error" };
        if (vehicleResult.IsError)
        {
            Snackbar.Add($"Error loading potentials: {vehicleResult.Error}", Severity.Error);
            VehicleTypes = [];
        }
        else
            VehicleTypes = vehicleResult.Value ?? [];
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
        var vehicleType = VehicleTypes?.FirstOrDefault(v => v.Id == vehicleTypeId);
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
        var response = await Client!.DeleteAsync($"/vehicle-types/{vehicleTypeId}");
        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result<object>>();
            Snackbar.Add(result?.Error ?? "Unknown error", Severity.Error);
        }
        else
            Snackbar.Add("Deleted", Severity.Success);
        await LoadVehicleTypes();
    }

    private bool FilterFunc(VehicleTypeDto vehicleType)
    {
        if (string.IsNullOrWhiteSpace(SearchString))
            return true;
        if (vehicleType.Name.Contains(SearchString, StringComparison.OrdinalIgnoreCase))
            return true;
        return false;
    }
}