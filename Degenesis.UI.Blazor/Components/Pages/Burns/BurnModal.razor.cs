using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Burns;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Burns;

public partial class BurnModal
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;
    [Parameter] public BurnDto Burn { get; set; } = new();

    private async Task SaveBurn()
    {
        HttpResponseMessage response;
        if (Burn.Id == Guid.Empty)
            response = await Client!.PostAsJsonAsync("/burns", Burn);
        else
            response = await Client!.PutAsJsonAsync($"/burns", Burn);

        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result<object>>();
            Snackbar.Add(result?.Error ?? "Unknown error", Severity.Error);
            return;
        }

        Snackbar.Add(Burn.Id == Guid.Empty ? "Created" : "Edited", Severity.Success);
        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel() => MudDialog.Cancel();
}