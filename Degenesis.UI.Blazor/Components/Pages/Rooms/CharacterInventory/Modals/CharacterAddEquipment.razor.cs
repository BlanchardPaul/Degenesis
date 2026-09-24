using Degenesis.Shared.DTOs.Characters.CRUD.Inventory;
using Degenesis.Shared.DTOs.Characters.Display;
using Degenesis.Shared.DTOs.Equipments;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Rooms.CharacterInventory.Modals;

public partial class CharacterAddEquipment
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;
    [Parameter] public CharacterDisplayDto Character { get; set; } = new();
    public List<EquipmentDto> Equipments { get; set; } = [];

    private HttpClient _client = new();
    private bool EquipmentsLoaded;

    protected override async Task OnInitializedAsync()
    {
        _client = await HttpClientService.GetClientAsync();
        Equipments = await _client.GetFromJsonAsync<List<EquipmentDto>>("/equipments") ?? [];
        EquipmentsLoaded = true;
    }

    private async Task AddCharacterEquipment(Guid equipmentId)
    {
        if (equipmentId == Guid.Empty)
        {
            Snackbar.Add("Please select an equipment first.", Severity.Warning);
            return;
        }
        var result = await _client.PostAsJsonAsync($"/character-equipments/", new CharacterEquipmentCreateDto { Id = Guid.NewGuid(), CharacterId = Character.Id, EquipmentId = equipmentId });
        if (!result.IsSuccessStatusCode)
            Snackbar.Add("Error while adding character equipment", Severity.Error);
        else
            Snackbar.Add("Character equipment added successfully.", Severity.Success);
        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel() => MudDialog.Cancel();
}
