using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Characters.CRUD;
using Degenesis.Shared.DTOs.Characters.Display;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;

namespace Degenesis.UI.Blazor.Components.Pages.Rooms;

public partial class RoomPage
{
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
}
