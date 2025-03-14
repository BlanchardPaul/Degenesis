using Degenesis.Shared.DTOs.Equipments;
using Degenesis.UI.Service.Features.Equipments;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Equipments;

public partial class EquipmentModal
{
    [Inject] private EquipmentService EquipmentService { get; set; } = default!;
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;

    [Parameter] public EquipmentDto Equipment { get; set; } = new();
    [Parameter] public List<EquipmentTypeDto> EquipmentTypes { get; set; } = new();

    protected override void OnParametersSet()
    {
        if (Equipment.EquipmentTypeId == Guid.Empty && EquipmentTypes.Any())
        {
            Equipment.EquipmentTypeId = EquipmentTypes.First().Id;
        }
    }

    private async Task SaveEquipment()
    {
        if (Equipment.Id == Guid.Empty)
            await EquipmentService.CreateEquipmentAsync(Equipment);
        else
            await EquipmentService.UpdateEquipmentAsync(Equipment);

        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel() => MudDialog.Cancel();
}