using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Characters.Display;
using Degenesis.UI.Blazor.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Rooms;

public partial class GmCharacterPage
{
    [Inject] private RoomConnectionService RoomConnection { get; set; } = default!;
    [Inject] private AuthenticationStateProvider AuthProvider { get; set; } = default!;

    [Parameter] public Guid CharacterId { get; set; }
    [Parameter] public string View { get; set; } = "sheet";

    private CharacterDisplayDto? Character;
    private HubConnection? hubConnection;
    private string userName = string.Empty;
    private bool loadError;

    private bool ShowInventory => View.Equals("inventory", StringComparison.OrdinalIgnoreCase);

    protected override async Task OnAuthenticatedInitializedAsync()
    {
        // Auth first: the dice modal needs to know who rolls
        var auth = await AuthProvider.GetAuthenticationStateAsync();
        userName = auth.User.FindFirst("unique_name")?.Value ?? string.Empty;

        await ReloadCharacter();

        // Connect to the room hub (joins the room group via the service)
        // Sequential on purpose: we need Character.IdRoom
        if (Character is not null)
            hubConnection = await RoomConnection.GetOrCreateAsync(Character.IdRoom, userName);

        // Hub listener so that if another player or the GM modified this character it's reloaded
        hubConnection?.On<Guid>("CharacterUpdated", async characterId =>
            {
                if (characterId == CharacterId)
                    await InvokeAsync(ReloadCharacter);
            });
    }

    private async Task ReloadCharacter()
    {
        loadError = false;
        var response = await Client!.GetFromJsonAsync<Result<CharacterDisplayDto>>($"/characters/gm/character/{CharacterId}")
            ?? new Result<CharacterDisplayDto> { IsError = true, Error = "Unknown error" };
        if (response.IsError)
        {
            loadError = true;
            Snackbar.Add("Error loading character: " + response.Error, Severity.Error);
        }
        else
        {
            Character = response.Value;
        }
        StateHasChanged();
    }

    private async Task OpenDiceRollModal()
    {
        if (Character is null || hubConnection is null)
            return;

        var parameters = new DialogParameters
        {
            { "Character", Character },
            { "HubConnection", hubConnection },
            { "IdRoom", Character.IdRoom }
        };
        var options = new DialogOptions
        {
            CloseButton = true,
            MaxWidth = MaxWidth.Small,
            FullWidth = true,
            BackdropClick = true
        };
        await DialogService.ShowAsync<DiceRollModal>("Roll dice", parameters, options);
    }
}