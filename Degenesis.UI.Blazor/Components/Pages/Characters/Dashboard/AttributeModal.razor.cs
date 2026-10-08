using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Characters.CRUD;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Characters.Dashboard;

public partial class AttributeModal
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;
    [Parameter] public AttributeDto Attribute { get; set; } = new();

    private async Task SaveAttribute()
    {
        HttpResponseMessage response;
        if (Attribute.Id == Guid.Empty)
            response = await Client!.PostAsJsonAsync("/attributes", Attribute);
        else
            response = await Client!.PutAsJsonAsync($"/attributes", Attribute);

        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result<object>>();
            Snackbar.Add(result?.Error ?? "Unknown error", Severity.Error);
            return;
        }

        Snackbar.Add(Attribute.Id == Guid.Empty ? "Created" : "Edited", Severity.Success);
        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel() => MudDialog.Cancel();

}