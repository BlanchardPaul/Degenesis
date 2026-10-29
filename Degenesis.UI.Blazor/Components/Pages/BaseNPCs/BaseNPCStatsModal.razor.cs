using Degenesis.Shared.DTOs;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.BaseNPCs;

public partial class BaseNPCStatsModal
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;
    [Parameter] public BaseNPCDto NPC { get; set; } = new();

    private async Task Save()
    {
        var response = await Client!.PutAsJsonAsync("/base-npcs/stats", NPC);
        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result<object>>();
            Snackbar.Add(result?.Error ?? "Unknown error", Severity.Error);
            return;
        }

        Snackbar.Add("Stats updated", Severity.Success);
        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel() => MudDialog.Cancel();
}