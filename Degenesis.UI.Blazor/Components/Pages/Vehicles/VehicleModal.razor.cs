using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Characters.CRUD;
using Degenesis.Shared.DTOs.Vehicles;
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
        HttpResponseMessage response;
        if (Vehicle.Id == Guid.Empty)
            response = await Client!.PostAsJsonAsync("/vehicles", Vehicle);
        else
            response = await Client!.PutAsJsonAsync($"/vehicles", Vehicle);

        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result<object>>();
            Snackbar.Add(result?.Error ?? "Unknown error", Severity.Error);
            return;
        }

        Snackbar.Add(Vehicle.Id == Guid.Empty ? "Created" : "Edited", Severity.Success);

        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel() => MudDialog.Cancel();
}