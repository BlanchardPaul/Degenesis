using Degenesis.Shared.DTOs.Characters.Display;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Rooms;

public partial class RoomPage
{
    [Parameter] public Guid IdRoom { get; set; }
    private CharacterDisplayDto? Character = new();
    private HttpClient _client = new();

    protected override async Task OnInitializedAsync()
    {
        _client = await HttpClientService.GetClientAsync();
        await ReloadCharacter();
    }

    private async Task ReloadCharacter()
    {
        Character = await _client.GetFromJsonAsync<CharacterDisplayDto>($"/characters/{IdRoom}");
        StateHasChanged();
    }
}
