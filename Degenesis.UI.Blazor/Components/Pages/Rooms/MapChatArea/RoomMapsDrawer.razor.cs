using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Rooms;
using Microsoft.AspNetCore.Components;

namespace Degenesis.UI.Blazor.Components.Pages.Rooms.MapChatArea;

public partial class RoomMapsDrawer
{
    [Parameter] public Guid IdRoom { get; set; }
    [Parameter] public RoomMapDto? SelectedMap { get; set; }
    [Parameter] public EventCallback<RoomMapDto> OnSelect { get; set; }
    [Parameter] public EventCallback OnClose { get; set; }

    private readonly List<RoomMapDto> _maps = [];
    private bool _isLoading = true;
    private string _imageVersion = Guid.NewGuid().ToString("N");
    private string ApiRoot => Client?.BaseAddress is null
        ? string.Empty
        : new Uri(Client.BaseAddress, "..").GetLeftPart(UriPartial.Authority);

    protected override async Task OnAuthenticatedInitializedAsync()
    {
        var result = await Client!.GetFromJsonAsync<Result<List<RoomMapDto>>>($"/room-maps/room/{IdRoom}");
        if (result is null || result.IsError)
        {
            Snackbar.Add(result?.Error ?? "Unable to load room maps", MudBlazor.Severity.Error);
        }
        else
        {
            _maps.AddRange(result.Value ?? []);
        }

        _isLoading = false;
    }

    private string GetMapImageUrl(RoomMapDto map)
        => $"{ApiRoot}/map-images/{Uri.EscapeDataString(map.FileName)}?v={_imageVersion}";
}
