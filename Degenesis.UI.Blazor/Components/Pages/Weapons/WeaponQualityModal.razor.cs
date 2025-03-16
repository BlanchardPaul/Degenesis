using Degenesis.Shared.DTOs.Weapons;
using Degenesis.UI.Service.Features.Weapons;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Weapons;

public partial class WeaponQualityModal
{
    [Inject] private WeaponQualityService WeaponQualityService { get; set; } = default!;
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;

    [Parameter] public WeaponQualityDto WeaponQuality { get; set; } = new();

    private async Task SaveWeaponQuality()
    {
        if (WeaponQuality.Id == Guid.Empty)
            await WeaponQualityService.CreateWeaponQualityAsync(WeaponQuality);
        else
            await WeaponQualityService.UpdateWeaponQualityAsync(WeaponQuality);

        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel() => MudDialog.Cancel();
}