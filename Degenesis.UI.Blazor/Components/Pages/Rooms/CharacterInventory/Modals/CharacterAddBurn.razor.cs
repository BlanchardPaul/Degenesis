using Degenesis.Shared.DTOs.Burns;
using Degenesis.Shared.DTOs.Characters.CRUD.Inventory;
using Degenesis.Shared.DTOs.Characters.Display;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Rooms.CharacterInventory.Modals;

public partial class CharacterAddBurn
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;
    [Parameter] public CharacterDisplayDto Character { get; set; } = new();
    public List<BurnDto>? Burns { get; set; }
    private string SearchString { get; set; } = "";

    protected override async Task OnAuthenticatedInitializedAsync()
    {
        Burns = await Client!.GetFromJsonAsync<List<BurnDto>>("/burns") ?? [];
    }

    private async Task AddCharacterBurn(Guid burnId)
    {
        if (burnId == Guid.Empty)
        {
            Snackbar.Add("Please select an burn first.", Severity.Warning);
            return;
        }

        var result = await Client!.PostAsJsonAsync($"/character-burns/", new CharacterBurnCreateDto { Id = Guid.NewGuid(), CharacterId = Character.Id, BurnId = burnId });
        if (!result.IsSuccessStatusCode)
            Snackbar.Add("Error while adding character burn", Severity.Error);
        else
            Snackbar.Add("Character burn added successfully.", Severity.Success);
        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel() => MudDialog.Cancel();

    private bool FilterFunc(BurnDto burn)
    {
        if (string.IsNullOrWhiteSpace(SearchString))
            return true;
        if (burn.Name.Contains(SearchString, StringComparison.OrdinalIgnoreCase))
            return true;
        return false;
    }
}