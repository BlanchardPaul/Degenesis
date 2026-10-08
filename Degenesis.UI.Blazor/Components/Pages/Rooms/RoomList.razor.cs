using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Rooms;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Rooms;

public partial class RoomList
{
    private List<RoomDisplayDto>? rooms;

    protected override async Task OnAuthenticatedInitializedAsync()
    {
        await LoadRooms();
    }
    private async Task LoadRooms()
    {
        var roomResult = await Client!.GetFromJsonAsync<Result<List<RoomDisplayDto>>>("/rooms") ?? new Result<List<RoomDisplayDto>> { IsError = true, Error = "Unknown error" };
        if (roomResult.IsError)
        {
            Snackbar.Add($"Error loading rooms: {roomResult.Error}", Severity.Error);
            rooms = [];
        }
        else
            rooms = roomResult.Value ?? [];
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
        var acceptResult = await Client!.GetFromJsonAsync<Result<object>>($"/rooms/acceptinvite/{roomId}") ?? new Result<object> { IsError = true, Error = "Unknown error" };
        if (acceptResult.IsError)
        {
            Snackbar.Add($"Error accepting invitation : {acceptResult.Error}", Severity.Error);
            rooms = [];
        }
        else
            Snackbar.Add("Accepted", Severity.Success);

        await LoadRooms();
        return;
    }

    private async Task ConfirmLeaveRoom(Guid roomId)
    {
        var parameters = new DialogParameters<ConfirmationDialog>
        {
            { "ContentText", "Leave room ? (character will be deleted)" }
        };

        var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Small };

        var dialog = await DialogService.ShowAsync<ConfirmationDialog>("Confirm", parameters, options);
        var result = await dialog.Result;

        if (result is not null && !result.Canceled)
        {
            await DeclineInvitation(roomId);
        }
    }

    private async Task DeclineInvitation(Guid roomId)
    {
        var acceptResult = await Client!.GetFromJsonAsync<Result<object>>($"/rooms/declineinvite/{roomId}") ?? new Result<object> { IsError = true, Error = "Unknown error" };
        if (acceptResult.IsError)
        {
            Snackbar.Add($"Error declining invitation : {acceptResult.Error}", Severity.Error);
            return;
        }
        Snackbar.Add("Declined", Severity.Success);

        await LoadRooms();
        return;
    }

    private async Task DeleteRoom(Guid roomId)
    {
        var deleteResult = await Client!.DeleteAsync($"/rooms/{roomId}");
        if (!deleteResult.IsSuccessStatusCode)
        {
            var result = await deleteResult.Content.ReadFromJsonAsync<Result<object>>();
            Snackbar.Add(result?.Error ?? "Unknown error", Severity.Error);
        }
        else
            Snackbar.Add("Deleted", Severity.Success);

        Snackbar.Add("Deleted", Severity.Success);
        await LoadRooms();
    }

    private void JoinRoom(Guid idRoom)
    {
        NavigationManager.NavigateTo($"/room/{idRoom}");
    }

    private void CreateCharacter(Guid roomId)
    {
        NavigationManager.NavigateTo($"/createcharacter/{roomId}");
    }

    private async Task ConfirmDeleteCharacter(Guid roomId)
    {
        var parameters = new DialogParameters<ConfirmationDialog>
        {
            { "ContentText", "Delete character ?" }
        };

        var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Small };

        var dialog = await DialogService.ShowAsync<ConfirmationDialog>("Confirm", parameters, options);
        var result = await dialog.Result;

        if (result is not null && !result.Canceled)
        {
            await DeleteCharacter(roomId);
        }
    }

    private async Task DeleteCharacter(Guid roomId)
    {
        var response = await Client!.DeleteAsync($"/characters/{roomId}");

        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result<object>>();
            Snackbar.Add(result?.Error ?? "Unknown error", Severity.Error);
        }
        else
            Snackbar.Add("Deleted", Severity.Success);

        await LoadRooms();
    }
}