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
        if (Weapon.Id == Guid.Empty)
        {
            var result = await Client!.PostAsJsonAsync("/weapons", Weapon);
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
            var result = await Client!.PutAsJsonAsync("/weapons", Weapon);
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