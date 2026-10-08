using Degenesis.Shared.DTOs;
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
        var result = await Client!.GetFromJsonAsync<Result<List<BurnDto>>>("/burns") ?? new Result<List<BurnDto>> { IsError = true, Error = "Unknown error" };
        if (result.IsError)
        {
            Snackbar.Add("Error loading burns: " + result.Error);
            Burns = [];
        }
        else
        {
            Burns = result.Value;
        }
    }

    private async Task AddCharacterBurn(Guid burnId)
    {
        if (burnId == Guid.Empty)
        {
            Snackbar.Add("Please select an burn first.", Severity.Warning);
            return;
        }

        var response = await Client!.PostAsJsonAsync($"/character-burns/", new CharacterBurnCreateDto { Id = Guid.NewGuid(), CharacterId = Character.Id, BurnId = burnId });
        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result<object>>();
            Snackbar.Add(result?.Error ?? "Unknown error", Severity.Error);
            return;
        }
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