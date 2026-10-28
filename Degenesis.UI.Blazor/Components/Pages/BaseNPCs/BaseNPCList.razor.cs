using Degenesis.Shared.DTOs;
using Degenesis.UI.Blazor.Extensions;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.BaseNPCs;

public partial class BaseNPCList : AuthenticatedComponentBase
{
    private List<BaseNPCDto>? npcs;
    private string SearchString = "";
    protected override async Task OnAuthenticatedInitializedAsync() {
        await ReloadNPCs();
    }

    private async Task ReloadNPCs()
    {
        var result = await Client!.GetFromJsonAsync<Result<List<BaseNPCDto>>>("/base-npcs") ?? new Result<List<BaseNPCDto>> { IsError = true, Error = "Unknown error" };
        if (result.IsError)
        {
            Snackbar.Add("Error loading NPCs: " + result.Error, Severity.Error);
            npcs = [];
        }
        else
            npcs = result.Value ?? [];
        StateHasChanged();
    }

    private async Task DeleteNPC(Guid id)
    {
        var response = await Client!.DeleteAsync($"/base-npcs/{id}");
        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result<object>>();
            Snackbar.Add(result?.Error ?? "Unknown error", Severity.Error);
            return;
        }
        Snackbar.Add("NPC deleted", Severity.Success);
        await ReloadNPCs();
    }

    private async Task OpenCreateDialog()
    {
        var options = new DialogOptions
        {
            CloseButton = true,
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            BackdropClick = false
        };
        var dialog = await DialogService.ShowAsync<BaseNPCModal>("Create NPC", options);
        var result = await dialog.Result;
        if (result is not null && !result.Canceled)
            await ReloadNPCs();
    }

    private async Task ShowBasicEditDialog(Guid npcId)
    {
        var npc = npcs?.FirstOrDefault(n => n.Id == npcId);
        if (npc != null)
        {
            var parameters = new DialogParameters
            {
                { "NPC", npc }
            };

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, BackdropClick = false };

            var dialog = await DialogService.ShowAsync<BaseNPCModal>("Edit NPC", parameters, options);
            var result = await dialog.Result;

            if (result is not null && !result.Canceled)
            {
                await ReloadNPCs();
            }
        }
    }

    private async Task ShowStatsEditDialog(Guid npcId)
    {
        var npc = npcs?.FirstOrDefault(n => n.Id == npcId);
        if (npc is null)
            return;

        var parameters = new DialogParameters
        {
            { "NPC", npc }
        };
        var options = new DialogOptions
        {
            CloseButton = true,
            MaxWidth = MaxWidth.ExtraLarge,
            FullWidth = true,
            BackdropClick = false
        };
        var dialog = await DialogService.ShowAsync<BaseNPCStatsModal>($"Edit stats - {npc.Name}", parameters, options);
        var result = await dialog.Result;
        if (result is not null && !result.Canceled)
            await ReloadNPCs();
    }

    private bool FilterFunc(BaseNPCDto npc)
    {
        if (string.IsNullOrWhiteSpace(SearchString))
            return true;
        if (npc.Name.Contains(SearchString, StringComparison.OrdinalIgnoreCase))
            return true;
        return false;
    }
}
