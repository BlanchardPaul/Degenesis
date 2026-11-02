using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.NPCs;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Rooms.NPC;

public partial class NpcAddFromBase
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;
    [Parameter] public Guid IdRoom { get; set; }

    private List<BaseNPCDto>? BaseNpcs { get; set; }
    private string SearchString { get; set; } = "";

    protected override async Task OnAuthenticatedInitializedAsync()
    {
        var result = await Client!.GetFromJsonAsync<Result<List<BaseNPCDto>>>("/base-npcs")
            ?? new Result<List<BaseNPCDto>> { IsError = true, Error = "Unknown error" };
        if (result.IsError)
        {
            Snackbar.Add("Error loading base NPCs: " + result.Error, Severity.Error);
            BaseNpcs = [];
        }
        else
        {
            BaseNpcs = result.Value;
        }
    }

    private async Task AddNpc(Guid baseNpcId)
    {
        var response = await Client!.PostAsJsonAsync($"/npcs/{baseNpcId}/room/{IdRoom}", new { });
        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result<object>>();
            Snackbar.Add(result?.Error ?? "Unknown error", Severity.Error);
            return;
        }
        Snackbar.Add("NPC added successfully.", Severity.Success);
        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel() => MudDialog.Cancel();

    private bool FilterFunc(BaseNPCDto baseNpc)
        => string.IsNullOrWhiteSpace(SearchString)
           || baseNpc.Name.Contains(SearchString, StringComparison.OrdinalIgnoreCase);
}