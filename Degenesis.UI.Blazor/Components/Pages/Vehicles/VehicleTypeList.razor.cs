using Degenesis.Shared.DTOs.Vehicles;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Vehicles;

public partial class VehicleTypeList
{
    private List<VehicleTypeDto>? vehicleTypes;
    private string SearchString = "";

    protected override async Task OnAuthenticatedInitializedAsync()
    {
        await LoadVehicleTypes();
    }

    private async Task LoadVehicleTypes()
    {
        vehicleTypes = await Client!.GetFromJsonAsync<List<VehicleTypeDto>>("/vehicle-types") ?? [];
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
        var result = await Client!.DeleteAsync($"/vehicle-types/{vehicleTypeId}");
        if (!result.IsSuccessStatusCode)
            Snackbar.Add("Error during deletion");
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