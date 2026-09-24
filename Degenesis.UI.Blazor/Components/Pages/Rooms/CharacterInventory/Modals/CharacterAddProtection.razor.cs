using Degenesis.Shared.DTOs.Characters.CRUD.Inventory;
using Degenesis.Shared.DTOs.Characters.Display;
using Degenesis.Shared.DTOs.Equipments;
using Degenesis.Shared.DTOs.Protections;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Rooms.CharacterInventory.Modals;

public partial class CharacterAddProtection
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;
    [Parameter] public CharacterDisplayDto Character { get; set; } = new();
    public List<ProtectionDto> Protections { get; set; } = [];

    private HttpClient _client = new();
    private bool ProtectionsLoaded;

    protected override async Task OnInitializedAsync()
    {
        _client = await HttpClientService.GetClientAsync();
        Protections = await _client.GetFromJsonAsync<List<ProtectionDto>>("/protections") ?? [];
        ProtectionsLoaded = true;
    }

    private async Task AddCharacterProtection(Guid protectionId)
    {
        if (protectionId == Guid.Empty)
        {
            Snackbar.Add("Please select a protection first.", Severity.Warning);
            return;
        }
        var result = await _client.PostAsJsonAsync($"/character-protections/", new CharacterProtectionCreateDto { Id = Guid.NewGuid(), CharacterId = Character.Id, ProtectionId = protectionId });
        if (!result.IsSuccessStatusCode)
            Snackbar.Add("Error while adding character protection", Severity.Error);
        else
            Snackbar.Add("Character protection added successfully.", Severity.Success);
        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel() => MudDialog.Cancel();
}
