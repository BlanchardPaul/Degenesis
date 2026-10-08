using Degenesis.Shared.DTOs;
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
        var weaponTypeResult = await Client!.GetFromJsonAsync<Result<List<WeaponTypeDto>>>("/weapon-types") ?? new Result<List<WeaponTypeDto>> { IsError = true, Error = "Unknown error" };
        if (weaponTypeResult.IsError)
        {
            Snackbar.Add($"Error loading weapon types: {weaponTypeResult.Error}", Severity.Error);
            weaponTypes = [];
        }
        else
            weaponTypes = weaponTypeResult.Value ?? [];
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
        var response = await Client!.DeleteAsync($"/weapon-types/{weaponTypeId}");
        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result<object>>();
            Snackbar.Add(result?.Error ?? "Unknown error", Severity.Error);
        }
        else
            Snackbar.Add("Deleted", Severity.Success);
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