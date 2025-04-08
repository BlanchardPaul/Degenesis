using Degenesis.Shared.DTOs.Characters;
using Degenesis.Shared.DTOs.Weapons;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Weapons;

public partial class WeaponList
{
    private List<WeaponDto>? weapons;
    private List<WeaponTypeDto> weaponTypes = [];
    private List<WeaponQualityDto> weaponQualities = [];
    private List<AttributeDto> attributes = [];
    private HttpClient _client = new();

    protected override async Task OnInitializedAsync()
    {
        _client = await HttpClientService.GetClientAsync();
        await LoadWeapons();
    }

    private async Task LoadWeapons()
    {
        weapons = await _client.GetFromJsonAsync<List<WeaponDto>>("/weapons") ?? [];
        weaponTypes = await _client.GetFromJsonAsync<List<WeaponTypeDto>>("/weapon-types") ?? [];
        weaponQualities = await _client.GetFromJsonAsync<List<WeaponQualityDto>>("/weapon-qualities") ?? [];
        attributes =  await _client.GetFromJsonAsync<List<AttributeDto>>("/attributes") ?? [];
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
        var result = await _client.DeleteAsync($"/weapons/{weaponId}");
        if (!result.IsSuccessStatusCode)
            Snackbar.Add("Error during deletion");
        else
            Snackbar.Add("Deleted");
        await LoadWeapons();
    }
}