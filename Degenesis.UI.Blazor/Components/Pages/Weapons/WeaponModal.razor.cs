using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Characters.CRUD;
using Degenesis.Shared.DTOs.Weapons;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Weapons;

public partial class WeaponModal
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;

    [Parameter] public WeaponDto Weapon { get; set; } = new();
    [Parameter] public List<WeaponTypeDto> WeaponTypes { get; set; } = [];
    [Parameter] public List<AttributeDto> Attributes { get; set; } = [];
    [Parameter] public List<SkillDto> Skills { get; set; } = [];
    [Parameter] public List<CultDto> Cults { get; set; } = [];
    private List<Guid> SelectedCultIds { get; set; } = [];

    protected override void OnParametersSet()
    {
        SelectedCultIds = [.. Weapon.Cults.Select(c => c.Id)];
        if (Weapon.WeaponTypeId == Guid.Empty && WeaponTypes.Count > 0)
        {
            Weapon.WeaponTypeId = WeaponTypes[0].Id;
        }
    }

    private Task OnCultsChanged(IEnumerable<Guid> selectedValues)
    {
        SelectedCultIds = [.. selectedValues];
        Weapon.Cults = [.. Cults.Where(c => SelectedCultIds.Contains(c.Id))];
        return Task.CompletedTask;
    }

    private async Task SaveWeapon()
    {
        HttpResponseMessage response;
        if (Weapon.Id == Guid.Empty)
            response = await Client!.PostAsJsonAsync("/weapons", Weapon);
        else
            response = await Client!.PutAsJsonAsync($"/weapons", Weapon);

        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result<object>>();
            Snackbar.Add(result?.Error ?? "Unknown error", Severity.Error);
            return;
        }

        Snackbar.Add(Weapon.Id == Guid.Empty ? "Created" : "Edited", Severity.Success);

        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel() => MudDialog.Cancel();
}