using Degenesis.Shared.DTOs.Vehicles;
using Degenesis.UI.Service.Features.Vehicles;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Vehicles;

public partial class VehicleModal
{
    [Inject] private VehicleService VehicleService { get; set; } = default!;
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;

    [Parameter] public VehicleDto Vehicle { get; set; } = new();
    [Parameter] public List<VehicleTypeDto> VehicleTypes { get; set; } = new();

    protected override void OnParametersSet()
    {
        if (Vehicle.VehicleTypeId == Guid.Empty && VehicleTypes.Count > 0)
        {
            Vehicle.VehicleTypeId = VehicleTypes[0].Id;
        }
    }

    private async Task SaveVehicle()
    {
        if (Vehicle.Id == Guid.Empty)
            await VehicleService.CreateVehicleAsync(Vehicle);
        else
            await VehicleService.UpdateVehicleAsync(Vehicle);

        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel() => MudDialog.Cancel();
}