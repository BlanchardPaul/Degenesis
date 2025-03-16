using Degenesis.Shared.DTOs.Characters;
using Degenesis.Shared.DTOs.Weapons;
using Degenesis.UI.Service.Features.Weapons;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Weapons;

public partial class WeaponModal
{
    [Inject] private WeaponService WeaponService { get; set; } = default!;
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;

    [Parameter] public WeaponDto Weapon { get; set; } = new();
    [Parameter] public List<WeaponTypeDto> WeaponTypes { get; set; } = [];
    [Parameter] public List<AttributeDto> Attributes { get; set; } = [];
    [Parameter] public List<WeaponQualityDto> WeaponQualities { get; set; } = [];

    private HashSet<Guid> SelectedQualityIds { get; set; } = [];

    protected override void OnParametersSet()
    {
        if (Weapon.WeaponTypeId == Guid.Empty && WeaponTypes.Count > 0)
        {
            Weapon.WeaponTypeId = WeaponTypes[0].Id;
        }

        SelectedQualityIds = new HashSet<Guid>(Weapon.Qualities.Select(q => q.Id));
    }

    private Task OnQualitiesChanged(IEnumerable<Guid> selectedValues)
    {
        SelectedQualityIds = new HashSet<Guid>(selectedValues);
        Weapon.Qualities = WeaponQualities.Where(q => SelectedQualityIds.Contains(q.Id)).ToList();
        return Task.CompletedTask;
    }

    private async Task SaveWeapon()
    {
        if (Weapon.Id == Guid.Empty)
            await WeaponService.CreateWeaponAsync(Weapon);
        else
            await WeaponService.UpdateWeaponAsync(Weapon);

        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel() => MudDialog.Cancel();
}