using Degenesis.Shared.DTOs.Equipments;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Equipments;

public partial class EquipmentList
{
    private List<EquipmentDto>? equipments;
    private List<EquipmentTypeDto> equipmentTypes = new();

    protected override async Task OnInitializedAsync()
    {
        await LoadEquipments();
    }

    private async Task LoadEquipments()
    {
        equipments = [.. (await EquipmentService.GetEquipmentsAsync())];
        equipmentTypes = [.. (await EquipmentTypeService.GetEquipmentTypesAsync())];
    }

    private async Task ShowCreateDialog()
    {
        var parameters = new DialogParameters
            {
                { "Equipment", new EquipmentDto() },
                { "EquipmentTypes", equipmentTypes }
            };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, BackdropClick = false };

        var dialog = await DialogService.ShowAsync<EquipmentModal>("Create Equipment", parameters, options);
        var result = await dialog.Result;

        if (result is not null && !result.Canceled)
        {
            await LoadEquipments();
        }
    }

    private async Task ShowEditDialog(Guid equipmentId)
    {
        var equipment = equipments?.FirstOrDefault(e => e.Id == equipmentId);
        if (equipment != null)
        {
            var parameters = new DialogParameters
                {
                    { "Equipment", equipment },
                    { "EquipmentTypes", equipmentTypes }
                };

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, BackdropClick = false };

            var dialog = await DialogService.ShowAsync<EquipmentModal>("Edit Equipment", parameters, options);
            var result = await dialog.Result;

            if (result is not null && !result.Canceled)
            {
                await LoadEquipments();
            }
        }
    }

    private async Task DeleteEquipment(Guid equipmentId)
    {
        await EquipmentService.DeleteEquipmentAsync(equipmentId);
        await LoadEquipments();
    }
}