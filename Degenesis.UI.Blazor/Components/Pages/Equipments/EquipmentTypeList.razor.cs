using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Equipments;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Equipments;

public partial class EquipmentTypeList
{
    private List<EquipmentTypeDto>? equipmentTypes;
    private string SearchString = "";

    protected override async Task OnAuthenticatedInitializedAsync()
    {
        await LoadEquipmentTypes();
    }

    private async Task LoadEquipmentTypes()
    {
        var equipmentTypeResult = await Client!.GetFromJsonAsync<Result<List<EquipmentTypeDto>>>("/equipment-types") ?? new Result<List<EquipmentTypeDto>> { IsError = true, Error = "Unknown error" };
        if (equipmentTypeResult.IsError)
        {
            Snackbar.Add($"Error loading equipment types: {equipmentTypeResult.Error}", Severity.Error);
            equipmentTypes = [];
        }
        else
            equipmentTypes = equipmentTypeResult.Value ?? [];
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
        var response = await Client!.DeleteAsync($"/equipment-types/{equipmentTypeId}");
        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result<object>>();
            Snackbar.Add(result?.Error ?? "Unknown error", Severity.Error);
        }
        else
            Snackbar.Add("Deleted", Severity.Success);

        await LoadEquipmentTypes();
    }

    private bool FilterFunc(EquipmentTypeDto equipmentType)
    {
        if (string.IsNullOrWhiteSpace(SearchString))
            return true;
        if (equipmentType.Name.Contains(SearchString, StringComparison.OrdinalIgnoreCase) == true)
            return true;
        return false;
    }
}