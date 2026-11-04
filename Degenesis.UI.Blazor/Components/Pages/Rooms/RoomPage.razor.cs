using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Characters.Display;
using Degenesis.Shared.DTOs.NPCs;
using Degenesis.Shared.DTOs.Rooms;
using Degenesis.UI.Blazor.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Rooms;

public partial class RoomPage
{
    [Inject] private RoomConnectionService RoomConnection { get; set; } = default!;
    [Inject] private AuthenticationStateProvider AuthProvider { get; set; } = default!;
    [Parameter] public Guid IdRoom { get; set; }
    private bool _showDiceRollModal = false;
    private RoomDisplayDto? room;
    private string? userName;
    private bool IsGM => room is not null && userName is not null && room.GMName == userName;

    // If user is player only load their character
    private CharacterDisplayDto? Character;

    // If user is GM load list of characters and npcs
    private List<CharacterDisplayForGMListDto> GmCharacters = [];
    private List<NPCDto> GmNpcs = [];

    private HubConnection? hubConnection;

    protected override async Task OnAuthenticatedInitializedAsync()
    {
        // Retrieve UserName from the Game Master display
        var auth = await AuthProvider.GetAuthenticationStateAsync();
        userName = auth.User.FindFirst("unique_name")?.Value;

        var roomResult = await Client!.GetFromJsonAsync<Result<RoomDisplayDto>>($"/rooms/{IdRoom}")
            ?? new Result<RoomDisplayDto> { IsError = true, Error = "Unknown error", Value = null };
        if (roomResult.IsError)
        {
            Snackbar.Add(roomResult.Error ?? "Unknown error", Severity.Error);
            return;
        }
        room = roomResult.Value;

        // Hub connection for the dice modal
        hubConnection = await RoomConnection.GetOrCreateAsync(IdRoom);

        // Hub listener so that if another player or the GM modified this character it's reloaded
        hubConnection!.On<Guid>("CharacterUpdated", async characterId =>
        {
            if (Character is not null && Character.Id == characterId)
                await InvokeAsync(ReloadCharacter);
        });

        // Hub listener so that if another player or the GM modified a NPC it's reloaded
        hubConnection!.On<Guid>("NpcUpdated", async npcId =>
        {
            if (IsGM)
                await InvokeAsync(LoadGmNpcs);
        });

        if (IsGM)
        {
            await LoadGmCharacters();
            await LoadGmNpcs();
        }
        else
            await ReloadCharacter();
    }

    private async Task ReloadCharacter()
    {
        var response = await Client!.GetFromJsonAsync<Result<CharacterDisplayDto>>($"/characters/{IdRoom}")
            ?? new Result<CharacterDisplayDto> { IsError = true, Error = "Unknown error" };
        if (response.IsError)
            Snackbar.Add("Error loading character: " + response.Error);
        else
            Character = response.Value;
        StateHasChanged();
    }

    private async Task LoadGmCharacters()
    {
        var response = await Client!.GetFromJsonAsync<Result<List<CharacterDisplayForGMListDto>>>($"/characters/gm/{IdRoom}")
            ?? new Result<List<CharacterDisplayForGMListDto>> { IsError = true, Error = "Unknown error" };
        if (response.IsError)
        {
            Snackbar.Add("Error loading characters: " + response.Error, Severity.Error);
            return;
        }
        GmCharacters = response.Value ?? [];
        StateHasChanged();
    }

    private async Task LoadGmNpcs()
    {
        var response = await Client!.GetFromJsonAsync<Result<List<NPCDto>>>($"/npcs/room/{IdRoom}")
            ?? new Result<List<NPCDto>> { IsError = true, Error = "Unknown error" };
        if (response.IsError)
        {
            Snackbar.Add("Error loading NPCs: " + response.Error, Severity.Error);
            return;
        }
        GmNpcs = response.Value ?? [];
        StateHasChanged();
    }

    private void OpenDiceRollModal()
    {
        _showDiceRollModal = true;
        StateHasChanged();
    }

    private void CloseDiceRollModal()
    {
        _showDiceRollModal = false;
        StateHasChanged();
    }
}