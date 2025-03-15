using Degenesis.Shared.DTOs.Vehicles;
using Degenesis.UI.Service.Features.Vehicles;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Vehicles;

public partial class VehicleTypeModal
{
    [Inject] private VehicleTypeService VehicleTypeService { get; set; } = default!;
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;

    [Parameter] public VehicleTypeDto VehicleType { get; set; } = new();

    private async Task SaveVehicleType()
    {
        if (VehicleType.Id == Guid.Empty)
            await VehicleTypeService.CreateVehicleTypeAsync(VehicleType);
        else
            await VehicleTypeService.UpdateVehicleTypeAsync(VehicleType);

        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel() => MudDialog.Cancel();
}