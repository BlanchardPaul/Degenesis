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
    [Parameter] public List<VehicleQualityDto> VehicleQualities { get; set; } = [];
    private List<Guid> SelectedQualityIds { get; set; } = [];
    private HttpClient _client = new();

    protected override async Task OnInitializedAsync()
    {
        SelectedQualityIds = [.. Vehicle.VehicleQualities.Select(v => v.Id)];
        _client = await HttpClientService.GetClientAsync();
    }

    protected override void OnParametersSet()
    {
        if (Vehicle.VehicleTypeId == Guid.Empty && VehicleTypes.Count > 0)
        {
            Vehicle.VehicleTypeId = VehicleTypes[0].Id;
        }
        SelectedQualityIds = [.. Vehicle.VehicleQualities.Select(v => v.Id)];
    }

    private Task OnQualitiesChanged(IEnumerable<Guid> selectedValues)
    {
        SelectedQualityIds = [.. selectedValues];
        Vehicle.VehicleQualities = VehicleQualities.Where(q => SelectedQualityIds.Contains(q.Id)).ToList();
        return Task.CompletedTask;
    }

    private async Task SaveVehicle()
    {
        if (Vehicle.Id == Guid.Empty)
        {
           var result = await _client.PostAsJsonAsync("/vehicles", Vehicle);
            if(!result.IsSuccessStatusCode)
                Snackbar.Add("Error during creation", Severity.Error);
            else
            {
                Snackbar.Add("Created", Severity.Success);
            }
        }

        else
        {
            var result = await _client.PutAsJsonAsync($"/vehicles", Vehicle);
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