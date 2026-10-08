using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Rooms;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Rooms;

public partial class RoomInviteDialog
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;
    [Parameter] public InvitationDto InvitationDto { get; set; } = new();

    private async Task Invite()
    {
        var response = await Client!.PostAsJsonAsync("/rooms/invite", InvitationDto);
        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result<object>>();
            Snackbar.Add(result?.Error ?? "Unknown error", Severity.Error);
            return;
        }
        else
        {
            Snackbar.Add("Invitation sent", Severity.Success);
            MudDialog.Close(DialogResult.Ok(true));
        }        
    }

    private void Cancel() => MudDialog.Cancel();
}