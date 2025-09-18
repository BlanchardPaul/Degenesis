using Degenesis.Shared.DTOs.Rooms;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Rooms;

public partial class RoomInviteDialog
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;
    [Parameter] public InvitationDto InvitationDto { get; set; } = new();

    private HttpClient _client = new();
    protected override async Task OnInitializedAsync()
    {
        _client = await HttpClientService.GetClientAsync();
    }

    private async Task Invite()
    {
        var result = await _client.PostAsJsonAsync("/rooms/invite", InvitationDto);
        if (result.IsSuccessStatusCode)
        {
            Snackbar.Add("Invitation sent", Severity.Success);
            MudDialog.Close(DialogResult.Ok(true));
        }
        else
            Snackbar.Add("Invitation failed, check user's name", Severity.Error);
        
    }

    private void Cancel() => MudDialog.Cancel();
}