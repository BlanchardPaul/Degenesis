using Degenesis.Shared.DTOs.Characters;
using Degenesis.Shared.DTOs.Weapons;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Weapons;

public partial class WeaponList
{
    private List<WeaponDto>? weapons;
    private List<WeaponTypeDto> weaponTypes = new();
    private List<WeaponQualityDto> weaponQualities = new();
    private List<AttributeDto> attributes = new();

    protected override async Task OnInitializedAsync()
    {
        await LoadWeapons();
    }

    private async Task LoadWeapons()
    {
        weapons = [.. (await WeaponService.GetWeaponsAsync())];
        weaponTypes = [.. (await WeaponTypeService.GetWeaponTypesAsync())];
        weaponQualities = [.. (await WeaponQualityService.GetWeaponQualitiesAsync())];
        attributes = [.. (await AttributeService.GetAttributesAsync())];
    }

    private async Task ShowCreateDialog()
    {
        var parameters = new DialogParameters
            {
                { "Weapon", new WeaponDto() },
                { "WeaponTypes", weaponTypes },
                { "WeaponQualities", weaponQualities },
                { "Attributes", attributes }
            };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, BackdropClick = false };

        var dialog = await DialogService.ShowAsync<WeaponModal>("Create Weapon", parameters, options);
        var result = await dialog.Result;

        if (result is not null && !result.Canceled)
        {
            await LoadWeapons();
        }
    }

    private async Task ShowEditDialog(Guid weaponId)
    {
        var weapon = weapons?.FirstOrDefault(w => w.Id == weaponId);
        if (weapon != null)
        {
            var parameters = new DialogParameters
                {
                    { "Weapon", weapon },
                    { "WeaponTypes", weaponTypes },
                    { "WeaponQualities", weaponQualities },
                    { "Attributes", attributes }
                };

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, BackdropClick = false };

            var dialog = await DialogService.ShowAsync<WeaponModal>("Edit Weapon", parameters, options);
            var result = await dialog.Result;

            if (result is not null && !result.Canceled)
            {
                await LoadWeapons();
            }
        }
    }

    private async Task DeleteWeapon(Guid weaponId)
    {
        await WeaponService.DeleteWeaponAsync(weaponId);
        await LoadWeapons();
    }
}