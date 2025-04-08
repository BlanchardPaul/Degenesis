using Degenesis.Shared.DTOs.Weapons;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Weapons;

public partial class WeaponTypeList
{
    private List<WeaponTypeDto>? weaponTypes;
    private HttpClient _client = new();

    protected override async Task OnInitializedAsync()
    {
        _client = await HttpClientService.GetClientAsync();
        await LoadWeaponTypes();
    }

    private async Task LoadWeaponTypes()
    {
        weaponTypes = await _client.GetFromJsonAsync<List<WeaponTypeDto>>("/weapon-types") ?? [];
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
        var result = await _client.DeleteAsync($"/weapon-types/{weaponTypeId}");
        if (!result.IsSuccessStatusCode)
            Snackbar.Add("Error during deletion");
        else
            Snackbar.Add("Deleted");
        await LoadWeaponTypes();
    }
}