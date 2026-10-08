using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Rooms;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Rooms;
public partial class RoomModal
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;
    [Parameter] public RoomDto Room { get; set; } = new();

    private async Task SaveRoom()
    {
        HttpResponseMessage response;
        if (Room.Id == Guid.Empty)
            response = await Client!.PostAsJsonAsync("/rooms", Room);
        else
            response = await Client!.PutAsJsonAsync("/rooms", Room);

        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result<object>>();
            Snackbar.Add(result?.Error ?? "Unknown error", Severity.Error);
            return;
        }

        Snackbar.Add(Room.Id == Guid.Empty ? "Created" : "Edited", Severity.Success);
        
        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel() => MudDialog.Cancel();
}