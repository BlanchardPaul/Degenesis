using Degenesis.Shared.DTOs;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.BaseNPCs;

public partial class BaseNPCModal
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;
    [Parameter] public BaseNPCDto NPC { get; set; } = new();

    private async Task SaveNPC()
    {
        HttpResponseMessage response;
        if (NPC.Id == Guid.Empty)
            response = await Client!.PostAsJsonAsync("/base-npcs", NPC);
        else
            response = await Client!.PutAsJsonAsync($"/base-npcs/{NPC.Id}", NPC);

        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result<object>>();
            Snackbar.Add(result?.Error ?? "Unknown error", Severity.Error);
            return;
        }

        Snackbar.Add(NPC.Id == Guid.Empty ? "Created" : "Edited", Severity.Success);
        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel() => MudDialog.Cancel();
}
