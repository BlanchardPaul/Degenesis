using Degenesis.Shared.DTOs;
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
    private string SearchString { get; set; } = "";

    protected override async Task OnAuthenticatedInitializedAsync()
    {
        var result = await Client!.GetFromJsonAsync<Result<List<VehicleDto>>>("/vehicles") ?? new Result<List<VehicleDto>> {IsError = true, Error = "Unknown error" };
        if (result.IsError)
        {
            Snackbar.Add("Error loading vehicles: " + result.Error);
            Vehicles = [];
        }
        else
        {
            Vehicles = result.Value ?? [];
        }
    }

    private async Task AddCharacterVehicle(Guid vehicleId)
    {
        if (vehicleId == Guid.Empty)
        {
            Snackbar.Add("Please select a vehicle first.", Severity.Warning);
            return;
        }

        var response = await Client!.PostAsJsonAsync($"/character-vehicles/", new { CharacterId = Character.Id, VehicleId = vehicleId });
        if(response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result<object>>();
            Snackbar.Add(result?.Error ?? "Unknown error", Severity.Error);
            return;
        }
        else
            Snackbar.Add("Character vehicle added successfully.", Severity.Success);

        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel() => MudDialog.Cancel();

    private bool FilterFunc(VehicleDto vehicle)
    {
        if (string.IsNullOrWhiteSpace(SearchString))
            return true;
        if (vehicle.Name.Contains(SearchString, StringComparison.OrdinalIgnoreCase))
            return true;
        return false;
    }
}
