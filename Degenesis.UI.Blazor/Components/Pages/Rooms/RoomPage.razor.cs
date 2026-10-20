using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Characters.Display;
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

    private RoomDisplayDto? room;
    private string? userName;
    private bool IsGM => room is not null && userName is not null && room.GMName == userName;

    private CharacterDisplayDto? Character;

    private List<CharacterDisplayForGMListDto> gmCharacters = [];
    private HubConnection? hubConnection;

    protected override async Task OnAuthenticatedInitializedAsync()
    {
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
        if (!string.IsNullOrEmpty(userName))
            hubConnection = await RoomConnection.GetOrCreateAsync(IdRoom, userName);

        // Hub listener so that if another player or the GM modified this character it's reloaded
        hubConnection!.On<Guid>("CharacterUpdated", async characterId =>
        {
            if (Character is not null && Character.Id == characterId)
                await InvokeAsync(ReloadCharacter);
        });

        if (IsGM)
            await LoadGmCharacters();
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
        gmCharacters = response.Value ?? [];
        StateHasChanged();
    }

    private static string GetGmCharacterUrl(Guid characterId, string view)
        => $"/gm-character/{characterId}/{view}";

    private async Task OpenDiceRollModal()
    {
        var parameters = new DialogParameters
        {
            { "Character", Character },
            { "HubConnection", hubConnection },
            { "IdRoom", IdRoom }
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