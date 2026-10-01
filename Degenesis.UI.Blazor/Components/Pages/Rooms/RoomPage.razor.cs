using Degenesis.Shared.DTOs.Characters.Display;
using Microsoft.AspNetCore.Components;

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
        Character = await Client!.GetFromJsonAsync<CharacterDisplayDto>($"/characters/{IdRoom}");
        StateHasChanged();
    }
}
