using Degenesis.Shared.DTOs;
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
    public List<EquipmentDto>? Equipments { get; set; }

    private string SearchString { get; set; } = "";

    protected override async Task OnAuthenticatedInitializedAsync()
    {
        var result = await Client!.GetFromJsonAsync<Result<List<EquipmentDto>>>("/equipments") ?? new Result<List<EquipmentDto>> { IsError = true, Error = "Unknown error" };
        if (result.IsError)
        {
            Snackbar.Add("Error loading equipments: " + result.Error);
            Equipments = [];
        }
        else
        {
            Equipments = result.Value;
        }
    }

    private async Task AddCharacterEquipment(Guid equipmentId)
    {
        if (equipmentId == Guid.Empty)
        {
            Snackbar.Add("Please select an equipment first.", Severity.Warning);
            return;
        }

        var response = await Client!.PostAsJsonAsync($"/character-equipments/", new CharacterEquipmentCreateDto { Id = Guid.NewGuid(), CharacterId = Character.Id, EquipmentId = equipmentId });
        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result<object>>();
            Snackbar.Add(result?.Error ?? "Unknown error", Severity.Error);
            return;
        }
        else
            Snackbar.Add("Character equipment added successfully.", Severity.Success);

        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel() => MudDialog.Cancel();

    private bool FilterFunc(EquipmentDto equipment)
    {
        if (string.IsNullOrWhiteSpace(SearchString))
            return true;
        if (equipment.Name.Contains(SearchString, StringComparison.OrdinalIgnoreCase))
            return true;
        return false;
    }
}
