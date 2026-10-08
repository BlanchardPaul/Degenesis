using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Equipments;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Equipments;

public partial class EquipmentTypeModal
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;

    [Parameter] public EquipmentTypeDto EquipmentType { get; set; } = new();

    private async Task SaveEquipmentType()
    {
        HttpResponseMessage response;
        if (EquipmentType.Id == Guid.Empty)
            response = await Client!.PostAsJsonAsync("/equipment-types", EquipmentType);
        else
            response = await Client!.PutAsJsonAsync("/equipment-types", EquipmentType);

        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result<object>>();
            Snackbar.Add(result?.Error ?? "Unknown error", Severity.Error);
            return;
        }

        Snackbar.Add(EquipmentType.Id == Guid.Empty ? "Created" : "Edited", Severity.Success);

        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel() => MudDialog.Cancel();
}
