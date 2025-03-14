using Degenesis.Shared.DTOs.Equipments;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Equipments;

public partial class EquipmentTypeList
{
    private List<EquipmentTypeDto>? equipmentTypes;

    protected override async Task OnInitializedAsync()
    {
        await LoadEquipmentTypes();
    }

    private async Task LoadEquipmentTypes()
    {
        equipmentTypes = [.. (await EquipmentTypeService.GetEquipmentTypesAsync())];
    }

    private async Task ShowCreateDialog()
    {
        var parameters = new DialogParameters
            {
                { "EquipmentType", new EquipmentTypeDto() }
            };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, BackdropClick = false };

        var dialog = await DialogService.ShowAsync<EquipmentTypeModal>("Create Equipment Type", parameters, options);
        var result = await dialog.Result;

        if (result is not null && !result.Canceled)
        {
            await LoadEquipmentTypes();
        }
    }

    private async Task ShowEditDialog(Guid equipmentTypeId)
    {
        var equipmentType = equipmentTypes?.FirstOrDefault(e => e.Id == equipmentTypeId);
        if (equipmentType != null)
        {
            var parameters = new DialogParameters
                {
                    { "EquipmentType", equipmentType }
                };

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, BackdropClick = false };

            var dialog = await DialogService.ShowAsync<EquipmentTypeModal>("Edit Equipment Type", parameters, options);
            var result = await dialog.Result;

            if (result is not null && !result.Canceled)
            {
                await LoadEquipmentTypes();
            }
        }
    }

    private async Task DeleteEquipmentType(Guid equipmentTypeId)
    {
        await EquipmentTypeService.DeleteEquipmentTypeAsync(equipmentTypeId);
        await LoadEquipmentTypes();
    }
}