using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.NPCs;
using Degenesis.UI.Blazor.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Rooms.NPC;

public partial class NpcEditPage
{
    [Inject] private RoomConnectionService RoomConnection { get; set; } = default!;
    [Inject] private AuthenticationStateProvider AuthProvider { get; set; } = default!;

    [Parameter] public Guid IdNpc { get; set; }

    private NPCDto? NPC { get; set; }
    private HubConnection? hubConnection;
    private Guid npcRoomId;

    protected override async Task OnAuthenticatedInitializedAsync()
    {
        var auth = await AuthProvider.GetAuthenticationStateAsync();
        var userName = auth.User.FindFirst("unique_name")?.Value ?? string.Empty;

        await ReloadNpc();

        if (npcRoomId != Guid.Empty)
            hubConnection = await RoomConnection.GetOrCreateAsync(npcRoomId, userName);

        hubConnection?.On<Guid>("NpcUpdated", async npcId =>
        {
            if (npcId == IdNpc)
                await InvokeAsync(ReloadNpc);
        });
    }

    private async Task ReloadNpc()
    {
        var response = await Client!.GetFromJsonAsync<Result<NPCDto>>($"/npcs/{IdNpc}")
            ?? new Result<NPCDto> { IsError = true, Error = "Unknown error" };
        if (response.IsError)
        {
            Snackbar.Add("Error loading NPC: " + response.Error, Severity.Error);
            return;
        }
        NPC = response.Value;
        npcRoomId = NPC!.IdRoom;
        StateHasChanged();
    }

    private async Task Save()
    {
        var response = await Client!.PutAsJsonAsync("/npcs", NPC);
        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result<object>>();
            Snackbar.Add(result?.Error ?? "Unknown error", Severity.Error);
            return;
        }

        Snackbar.Add("NPC updated", Severity.Success);
    }
}