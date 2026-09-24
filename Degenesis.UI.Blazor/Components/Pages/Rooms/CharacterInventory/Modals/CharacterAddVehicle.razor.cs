using Degenesis.Shared.DTOs.Characters.Display;
using Degenesis.Shared.DTOs.Vehicles;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Rooms.CharacterInventory.Modals;

public partial class CharacterAddVehicle
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;
    [Parameter] public CharacterDisplayDto Character { get; set; } = new();
    public List<VehicleDto> Vehicles { get; set; } = [];

    private HttpClient _client = new();
    private bool VehiclesLoaded;

    protected override async Task OnInitializedAsync()
    {
        _client = await HttpClientService.GetClientAsync();
        Vehicles = await _client.GetFromJsonAsync<List<VehicleDto>>("/vehicles") ?? [];
        VehiclesLoaded = true;
    }

    private async Task AddCharacterVehicle(Guid vehicleId)
    {
        if (vehicleId == Guid.Empty)
        {
            Snackbar.Add("Please select a vehicle first.", Severity.Warning);
            return;
        }
        var result = await _client.PostAsJsonAsync($"/character-vehicles/", new { CharacterId = Character.Id, VehicleId = vehicleId });
        if (!result.IsSuccessStatusCode)
            Snackbar.Add("Error while adding character vehicle", Severity.Error);
        else
            Snackbar.Add("Character vehicle added successfully.", Severity.Success);
        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel() => MudDialog.Cancel();
}
