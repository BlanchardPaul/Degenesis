using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Weapons;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Weapons;

public partial class WeaponTypeModal
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;

    [Parameter] public WeaponTypeDto WeaponType { get; set; } = new();

    private async Task SaveWeaponType()
    {
        HttpResponseMessage response;
        if (WeaponType.Id == Guid.Empty)
            response = await Client!.PostAsJsonAsync($"/weapon-types", WeaponType);
        else
            response = await Client!.PutAsJsonAsync($"/weapon-types", WeaponType);

        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result<object>>();
            Snackbar.Add(result?.Error ?? "Unknown error", Severity.Error);
            return;
        }

        Snackbar.Add(WeaponType.Id == Guid.Empty ? "Created" : "Edited", Severity.Success);

        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel() => MudDialog.Cancel();
}