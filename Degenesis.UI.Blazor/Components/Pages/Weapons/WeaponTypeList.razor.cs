using Degenesis.Shared.DTOs.Weapons;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Weapons;

public partial class WeaponTypeList
{
    private List<WeaponTypeDto>? weaponTypes;
    private string SearchString = "";
    protected override async Task OnAuthenticatedInitializedAsync()
    {
        await LoadWeaponTypes();
    }

    private async Task LoadWeaponTypes()
    {
        weaponTypes = await Client!.GetFromJsonAsync<List<WeaponTypeDto>>("/weapon-types") ?? [];
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
        var result = await Client!.DeleteAsync($"/weapon-types/{weaponTypeId}");
        if (!result.IsSuccessStatusCode)
            Snackbar.Add("Error during deletion");
        else
            Snackbar.Add("Deleted");
        await LoadWeaponTypes();
    }

    private bool FilterFunc(WeaponTypeDto weaponType)
    {
        if (string.IsNullOrWhiteSpace(SearchString))
            return true;
        if (weaponType.Name.Contains(SearchString, StringComparison.OrdinalIgnoreCase))
            return true;
        return false;
    }
}