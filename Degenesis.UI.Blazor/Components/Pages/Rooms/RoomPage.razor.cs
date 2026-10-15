using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Characters.Display;
using Degenesis.UI.Blazor.Extensions;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Rooms;

public partial class RoomPage
{
    [Inject] private RoomConnectionService RoomConnection { get; set; } = default!;
    [Parameter] public Guid IdRoom { get; set; }
    private CharacterDisplayDto? Character = null;

    protected override async Task OnAuthenticatedInitializedAsync()
    {
        await ReloadCharacter();
    }

    private async Task ReloadCharacter()
    {
        var response = await Client!.GetFromJsonAsync<Result<CharacterDisplayDto>>($"/characters/{IdRoom}") ?? new Result<CharacterDisplayDto> { IsError = true, Error = "Unknown error" };
        if (response.IsError) {
            Snackbar.Add("Error loading character: " + response.Error);
        }else
            Character = response.Value;
        StateHasChanged();
    }

    private async Task OpenDiceRollModal()
    {
        var parameters = new DialogParameters
        {
            { "Character", Character },
            { "HubConnection", RoomConnection.Connection },
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
