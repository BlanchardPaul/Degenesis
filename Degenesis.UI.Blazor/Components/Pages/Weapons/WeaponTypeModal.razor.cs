using Degenesis.Shared.DTOs.Weapons;
using Degenesis.UI.Service.Features.Weapons;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Weapons;

public partial class WeaponTypeModal
{
    [Inject] private WeaponTypeService WeaponTypeService { get; set; } = default!;
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;

    [Parameter] public WeaponTypeDto WeaponType { get; set; } = new();

    private async Task SaveWeaponType()
    {
        if (WeaponType.Id == Guid.Empty)
            await WeaponTypeService.CreateWeaponTypeAsync(WeaponType);
        else
            await WeaponTypeService.UpdateWeaponTypeAsync(WeaponType);

        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel() => MudDialog.Cancel();
}