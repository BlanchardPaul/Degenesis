using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Vehicles;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Vehicles;

public partial class VehicleTypeModal
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;

    [Parameter] public VehicleTypeDto VehicleType { get; set; } = new();

    private async Task SaveVehicleType()
    {
        HttpResponseMessage response;
        if (VehicleType.Id == Guid.Empty)
            response = await Client!.PostAsJsonAsync($"/vehicle-types", VehicleType);
        else
            response = await Client!.PostAsJsonAsync($"/vehicle-types", VehicleType);

        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result<object>>();
            Snackbar.Add(result?.Error ?? "Unknown error", Severity.Error);
            return;
        }

        Snackbar.Add(VehicleType.Id == Guid.Empty ? "Created" : "Edited", Severity.Success);

        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel() => MudDialog.Cancel();
}