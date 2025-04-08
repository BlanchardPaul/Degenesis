using Degenesis.Shared.DTOs.Rooms;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Rooms;

public partial class RoomList
{
    [Inject] private IDialogService DialogService { get; set; } = default!;
    private List<RoomDisplayDto>? rooms;
    private HttpClient _client = new();

    protected override async Task OnInitializedAsync()
    {
        _client = await HttpClientService.GetClientAsync();
        await LoadRooms();
    }
    private async Task LoadRooms()
    {
        rooms = await _client.GetFromJsonAsync<List<RoomDisplayDto>>("/rooms") ?? [];
    }

    private async Task ShowCreateDialog()
    {
        var parameters = new DialogParameters { { "Room", new RoomDto() } };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, BackdropClick = false };

        var dialog = await DialogService.ShowAsync<RoomModal>("Create Room", parameters, options);
        var result = await dialog.Result;

        if (result is not null && !result.Canceled)
        {
            await LoadRooms();
        }
    }

    private async Task ShowEditDialog(Guid roomId)
    {
        var room = rooms?.FirstOrDefault(a => a.Id == roomId);
        if (room != null)
        {
            var parameters = new DialogParameters { { "Room", room } };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, BackdropClick = false };

            var dialog = await DialogService.ShowAsync<RoomModal>("Edit Room", parameters, options);
            var result = await dialog.Result;

            if (result is not null && !result.Canceled)
            {
                await LoadRooms();
            }
        }
    }

    private async Task ShowInviteDialog(Guid idRoom)
    {
        var parameters = new DialogParameters { { "InvitationDto", new InvitationDto {IdRoom = idRoom } } };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, BackdropClick = false };

        var dialog = await DialogService.ShowAsync<RoomInviteDialog>("Invite User", parameters, options);
        var result = await dialog.Result;

        if (result is not null && !result.Canceled)
        {
            await LoadRooms();
        }
    }

    private async Task AcceptInvitation(Guid roomId)
    {
        var acceptResult = await _client.GetAsync($"/rooms/acceptinvite/{roomId}");
        if (!acceptResult.IsSuccessStatusCode)
        {
            Snackbar.Add("Acceptation failed", Severity.Error);
            return;
        }
        Snackbar.Add("Accepted", Severity.Success);

        await LoadRooms();
        return;
    }

    private async Task DeclineInvitation(Guid roomId)
    {
        var acceptResult = await _client.GetAsync($"/rooms/declineinvite/{roomId}");
        if (!acceptResult.IsSuccessStatusCode)
        {
            Snackbar.Add("Decline failed", Severity.Error);
            return;
        }
        Snackbar.Add("Declined", Severity.Success);

        await LoadRooms();
        return;
    }

    private async Task DeleteRoom(Guid roomId)
    {
        var deleteResult = await _client.DeleteAsync($"/rooms/{roomId}");
        if (!deleteResult.IsSuccessStatusCode) {
            Snackbar.Add("Delete failed", Severity.Error);
            return;
        }

        Snackbar.Add("Deleted", Severity.Success);
        await LoadRooms();
    }
}