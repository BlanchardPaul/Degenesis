using Degenesis.Shared.DTOs.Characters.CRUD;
using Degenesis.Shared.DTOs.Vehicles;
using Degenesis.Shared.DTOs.Weapons;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Vehicles;

public partial class VehicleModal
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;

    [Parameter] public VehicleDto Vehicle { get; set; } = new();
    [Parameter] public List<CultDto> Cults { get; set; } = [];
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
        {
           var result = await Client!.PostAsJsonAsync("/vehicles", Vehicle);
            if(!result.IsSuccessStatusCode)
                Snackbar.Add("Error during creation", Severity.Error);
            else
            {
                Snackbar.Add("Created", Severity.Success);
            }
        }

        else
        {
            var result = await Client!.PutAsJsonAsync($"/vehicles", Vehicle);
            if (!result.IsSuccessStatusCode)
                Snackbar.Add("Error during edition", Severity.Error);
            else
            {
                Snackbar.Add("Edited", Severity.Success);
            }
        }
        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel() => MudDialog.Cancel();
}