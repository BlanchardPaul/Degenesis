using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Characters.CRUD;
using Degenesis.Shared.DTOs.Equipments;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Equipments;

public partial class EquipmentModal
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;

    [Parameter] public EquipmentDto Equipment { get; set; } = new();
    [Parameter] public List<EquipmentTypeDto> EquipmentTypes { get; set; } = [];
    [Parameter] public List<CultDto> Cults { get; set; } = [];
    private List<Guid> SelectedCultIds { get; set; } = [];

    protected override void OnParametersSet()
    {
        SelectedCultIds = [.. Equipment.Cults.Select(c => c.Id)];

        if (Equipment.EquipmentTypeId == Guid.Empty && EquipmentTypes.Count != 0)
        {
            Equipment.EquipmentTypeId = EquipmentTypes.First().Id;
        }
    }

    private Task OnCultsChanged(IEnumerable<Guid> selectedValues)
    {
        SelectedCultIds = [.. selectedValues];
        Equipment.Cults = [.. Cults.Where(c => SelectedCultIds.Contains(c.Id))];
        return Task.CompletedTask;
    }

    private async Task SaveEquipment()
    {
        HttpResponseMessage response;
        if (Equipment.Id == Guid.Empty)
            response = await Client!.PostAsJsonAsync("/equipments", Equipment);
        else
            response = await Client!.PutAsJsonAsync("/equipments", Equipment);

        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result<object>>();
            Snackbar.Add(result?.Error ?? "Unknown error", Severity.Error);
        }
        else
            Snackbar.Add("Deleted", Severity.Success);

        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel() => MudDialog.Cancel();
}