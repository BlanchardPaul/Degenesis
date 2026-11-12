using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.NPCs;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Rooms.NPC;

public partial class RoomGmNpcList
{
    [Parameter] 
    public Guid IdRoom { get; set; }

    [Parameter]
    public List<NPCDto> GmNpcs { get; set; } = [];

    [Parameter]
    public EventCallback OnRequestParentReload { get; set; }

    private string SearchString { get; set; } = "";
    private List<NPCDto>? _avatarNpcs;
    private string _avatarVersion = Guid.NewGuid().ToString("N");
    private string ApiRoot => Client?.BaseAddress is null
        ? string.Empty
        : new Uri(Client.BaseAddress, "..").GetLeftPart(UriPartial.Authority);

    protected override void OnParametersSet()
    {
        if (!ReferenceEquals(GmNpcs, _avatarNpcs))
        {
            _avatarNpcs = GmNpcs;
            _avatarVersion = Guid.NewGuid().ToString("N");
        }
    }

    private string GetNpcAvatarUrl(NPCDto npc)
        => $"{ApiRoot}/npc-avatars/{npc.AvatarName}?v={_avatarVersion}";

    private async Task OpenAddNpcModal()
    {
        var parameters = new DialogParameters
        {
            { "IdRoom", IdRoom }
        };
        var options = new DialogOptions
        {
            CloseButton = true,
            MaxWidth = MaxWidth.ExtraLarge,
            FullWidth = true,
            BackdropClick = true
        };

        var dialog = await DialogService.ShowAsync<NpcAddFromBase>("Add an NPC to the room", parameters, options);
        var result = await dialog.Result;

        if (result is not null && !result.Canceled)
            await OnRequestParentReload.InvokeAsync();
    }

    private async Task UpdateNpcBasicInfos(NPCDto nPCDto)
    {
        if (nPCDto is null)
            return;

        var response = await Client!.PutAsJsonAsync($"/npcs/", nPCDto);
        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result<object>>();
            Snackbar.Add(result?.Error ?? "Unknown error", Severity.Error);
        }
    }

    private async Task ConfirmDeleteNpc(Guid npcId)
    {
        var parameters = new DialogParameters<ConfirmationDialog>
        {
            { "ContentText", "Delete npc ?" }
        };

        var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Small };

        var dialog = await DialogService.ShowAsync<ConfirmationDialog>("Confirm", parameters, options);
        var result = await dialog.Result;

        if (result is not null && !result.Canceled)
        {
            await DeleteNpc(npcId);
        }
    }

    private async Task DeleteNpc(Guid npcId)
    {
        var result = await Client!.DeleteAsync($"/npcs/{npcId}");
        if (!result.IsSuccessStatusCode)
            Snackbar.Add("Error during deletion", Severity.Error);
        else
        {
            await OnRequestParentReload.InvokeAsync();
            StateHasChanged();
        }
    }

    private bool FilterFunc(NPCDto npc)
    => string.IsNullOrWhiteSpace(SearchString)
       || npc.Name.Contains(SearchString, StringComparison.OrdinalIgnoreCase);
    private static string GetGmNpcUrl(Guid npcId)
    => $"/gm-npc/{npcId}";

}