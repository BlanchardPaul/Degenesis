using Degenesis.Shared.DTOs.Characters.CRUD.Inventory;
using Degenesis.Shared.DTOs.Characters.Display;
using Degenesis.Shared.DTOs.Protections;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Rooms.CharacterInventory.Modals;

public partial class CharacterAddProtection
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;
    [Parameter] public CharacterDisplayDto Character { get; set; } = new();
    public List<ProtectionDto>? Protections { get; set; }
    private string SearchString { get; set; } = "";

    protected override async Task OnAuthenticatedInitializedAsync()
    {
        Protections = await Client!.GetFromJsonAsync<List<ProtectionDto>>("/protections") ?? [];
    }

    private async Task AddCharacterProtection(Guid protectionId)
    {
        if (protectionId == Guid.Empty)
        {
            Snackbar.Add("Please select a protection first.", Severity.Warning);
            return;
        }
        var result = await Client!.PostAsJsonAsync($"/character-protections/", new CharacterProtectionCreateDto { Id = Guid.NewGuid(), CharacterId = Character.Id, ProtectionId = protectionId });
        if (!result.IsSuccessStatusCode)
            Snackbar.Add("Error while adding character protection", Severity.Error);
        else
            Snackbar.Add("Character protection added successfully.", Severity.Success);
        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel() => MudDialog.Cancel();

    private bool FilterFunc(ProtectionDto protection)
    {
        if (string.IsNullOrWhiteSpace(SearchString))
            return true;
        if (protection.Name.Contains(SearchString, StringComparison.OrdinalIgnoreCase))
            return true;
        return false;
    }
}
