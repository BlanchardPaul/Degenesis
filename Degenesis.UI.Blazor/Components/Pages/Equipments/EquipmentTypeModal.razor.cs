using Degenesis.Shared.DTOs.Equipments;
using Degenesis.UI.Service.Features.Equipments;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Equipments;

public partial class EquipmentTypeModal
{
    [Inject] private EquipmentTypeService EquipmentTypeService { get; set; } = default!;
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;

    [Parameter] public EquipmentTypeDto EquipmentType { get; set; } = new();

    private async Task SaveEquipmentType()
    {
        if (EquipmentType.Id == Guid.Empty)
            await EquipmentTypeService.CreateEquipmentTypeAsync(EquipmentType);
        else
            await EquipmentTypeService.UpdateEquipmentTypeAsync(EquipmentType);

        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel() => MudDialog.Cancel();
}
