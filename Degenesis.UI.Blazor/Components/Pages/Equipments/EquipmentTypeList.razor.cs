using Degenesis.Shared.DTOs.Equipments;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Equipments;

public partial class EquipmentTypeList
{
    private List<EquipmentTypeDto>? equipmentTypes;
    private HttpClient _client = new();

    protected override async Task OnInitializedAsync()
    {
        _client = await HttpClientService.GetClientAsync();
        await LoadEquipmentTypes();
    }

    private async Task LoadEquipmentTypes()
    {
        equipmentTypes = await _client.GetFromJsonAsync<List<EquipmentTypeDto>>("/equipment-types") ?? [];
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
        var result = await _client.DeleteAsync($"/equipment-types/{equipmentTypeId}");
        if (!result.IsSuccessStatusCode)
            Snackbar.Add("Error during deletion");
        else
            Snackbar.Add("Deleted");
        await LoadEquipmentTypes();
    }
}