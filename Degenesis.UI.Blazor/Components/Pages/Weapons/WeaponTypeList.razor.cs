using Degenesis.Shared.DTOs.Weapons;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Weapons;

public partial class WeaponTypeList
{
    private List<WeaponTypeDto>? weaponTypes;

    protected override async Task OnInitializedAsync()
    {
        await LoadWeaponTypes();
    }

    private async Task LoadWeaponTypes()
    {
        weaponTypes = [.. (await WeaponTypeService.GetWeaponTypesAsync())];
    }

    private async Task ShowCreateDialog()
    {
        var parameters = new DialogParameters
            {
                { "WeaponType", new WeaponTypeDto() }
            };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, BackdropClick = false };

        var dialog = await DialogService.ShowAsync<WeaponTypeModal>("Create Weapon Type", parameters, options);
        var result = await dialog.Result;

        if (result is not null && !result.Canceled)
        {
            await LoadWeaponTypes();
        }
    }

    private async Task ShowEditDialog(Guid weaponTypeId)
    {
        var weaponType = weaponTypes?.FirstOrDefault(w => w.Id == weaponTypeId);
        if (weaponType != null)
        {
            var parameters = new DialogParameters
                {
                    { "WeaponType", weaponType }
                };

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, BackdropClick = false };

            var dialog = await DialogService.ShowAsync<WeaponTypeModal>("Edit Weapon Type", parameters, options);
            var result = await dialog.Result;

            if (result is not null && !result.Canceled)
            {
                await LoadWeaponTypes();
            }
        }
    }

    private async Task DeleteWeaponType(Guid weaponTypeId)
    {
        await WeaponTypeService.DeleteWeaponTypeAsync(weaponTypeId);
        await LoadWeaponTypes();
    }
}