using Degenesis.Shared.DTOs.Characters.CRUD;
using Degenesis.Shared.DTOs.Equipments;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Equipments;

public partial class EquipmentList
{
    private List<EquipmentDto>? Equipments;
    private List<EquipmentTypeDto> EquipmentTypes = [];
    private List<CultDto> Cults = [];
    private HttpClient _client = new();

    protected override async Task OnInitializedAsync()
    {
        _client = await HttpClientService.GetClientAsync();
        await LoadEquipments();
    }

    private async Task LoadEquipments()
    {
        Equipments = await _client.GetFromJsonAsync<List<EquipmentDto>>("/equipments") ?? [];
        EquipmentTypes = await _client.GetFromJsonAsync<List<EquipmentTypeDto>>("/equipment-types") ?? [];
        Cults = await _client.GetFromJsonAsync<List<CultDto>>("/cults") ?? [];
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
        var result = await _client.DeleteAsync($"/equipments/{equipmentId}");
        if (!result.IsSuccessStatusCode)
            Snackbar.Add("Error during deletion");
        else
            Snackbar.Add("Deleted");
        await LoadEquipments();
    }
}