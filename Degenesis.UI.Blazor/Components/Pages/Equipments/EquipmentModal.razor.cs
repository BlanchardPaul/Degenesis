using Degenesis.Shared.DTOs.Equipments;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Equipments;

public partial class EquipmentModal
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;

    [Parameter] public EquipmentDto Equipment { get; set; } = new();
    [Parameter] public List<EquipmentTypeDto> EquipmentTypes { get; set; } = new();
    private HttpClient _client = new();

    protected override async Task OnInitializedAsync()
    {
        _client = await HttpClientService.GetClientAsync();
    }

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
        {
            var result = await _client.PostAsJsonAsync("/equipments", Equipment);
            if (!result.IsSuccessStatusCode)
                Snackbar.Add("Error during creation", Severity.Error);
            else
            {
                Snackbar.Add("Created", Severity.Success);
                MudDialog.Close(DialogResult.Ok(true));
            }
        }
        else
        {
            var result = await _client.PutAsJsonAsync("/equipments", Equipment);
            if (!result.IsSuccessStatusCode)
                Snackbar.Add("Error during edition", Severity.Error);
            else
            {
                Snackbar.Add("Edited", Severity.Success);
                MudDialog.Close(DialogResult.Ok(true));
            }
        }
        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel() => MudDialog.Cancel();
}