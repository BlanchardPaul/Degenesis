using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Characters.CRUD;
using Degenesis.Shared.DTOs.Equipments;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Equipments;

public partial class EquipmentList
{
    private List<EquipmentDto>? Equipments;
    private List<EquipmentTypeDto> EquipmentTypes = [];
    private List<CultDto> Cults = [];
    private string SearchString = "";

    protected override async Task OnAuthenticatedInitializedAsync()
    {
        await LoadEquipments();
    }

    private async Task LoadEquipments()
    {
        var equipmentResult = await Client!.GetFromJsonAsync<Result<List<EquipmentDto>>>("/equipments") ?? new Result<List<EquipmentDto>> { IsError = true, Error = "Unknown error" };
        if (equipmentResult.IsError)
        {
            Snackbar.Add($"Error loading equipments: {equipmentResult.Error}", Severity.Error);
            Equipments = [];
        }
        else
            Equipments = equipmentResult.Value ?? [];
        
        var equipmentTypesResult = await Client!.GetFromJsonAsync<Result<List<EquipmentTypeDto>>>("/equipment-types") ?? new Result<List<EquipmentTypeDto>> { IsError = true, Error = "Unknown error" };
        if (equipmentTypesResult.IsError)
        {
            Snackbar.Add($"Error loading equipment types: {equipmentTypesResult.Error}", Severity.Error);
            EquipmentTypes = [];
        }
        else
            EquipmentTypes = equipmentTypesResult.Value ?? [];

        var cultsResult = await Client!.GetFromJsonAsync<Result<List<CultDto>>>("/cults") ?? new Result<List<CultDto>> { IsError = true, Error = "Unknown error" };
        if (cultsResult.IsError)
        {
            Snackbar.Add($"Error loading cults: {cultsResult.Error}", Severity.Error);
            Cults = [];
        }
        else
            Cults = cultsResult.Value ?? [];

    }

    private async Task ShowCreateDialog()
    {
        var parameters = new DialogParameters
            {
                { "Equipment", new EquipmentDto() },
                { "EquipmentTypes", EquipmentTypes },
                { "Cults", Cults }
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
        var equipment = Equipments?.FirstOrDefault(e => e.Id == equipmentId);
        if (equipment != null)
        {
            var parameters = new DialogParameters
                {
                    { "Equipment", equipment },
                    { "EquipmentTypes", EquipmentTypes },
                    { "Cults", Cults }
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
        var response = await Client!.DeleteAsync($"/equipments/{equipmentId}");
        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result<object>>();
            Snackbar.Add(result?.Error ?? "Unknown error", Severity.Error);
        }
        else
            Snackbar.Add("Deleted", Severity.Success);

        await LoadEquipments();
    }

    private bool FilterFunc(EquipmentDto equipment)
    {
        if (string.IsNullOrWhiteSpace(SearchString))
            return true;
        if (equipment.Name.Contains(SearchString, StringComparison.OrdinalIgnoreCase))
            return true;
        return false;
    }
}