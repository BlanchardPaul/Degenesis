using Degenesis.Shared.DTOs.Weapons;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Weapons;

public partial class WeaponQualityList
{
    private List<WeaponQualityDto>? weaponQualities;
    private HttpClient _client = new();

    protected override async Task OnInitializedAsync()
    {
        _client = await HttpClientService.GetClientAsync();
        await LoadWeaponQualities();
    }

    private async Task LoadWeaponQualities()
    {
        weaponQualities = await _client.GetFromJsonAsync<List<WeaponQualityDto>>("/weapon-qualities") ?? [];
    }

    private async Task ShowCreateDialog()
    {
        var parameters = new DialogParameters
            {
                { "WeaponQuality", new WeaponQualityDto() }
            };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, BackdropClick = false };

        var dialog = await DialogService.ShowAsync<WeaponQualityModal>("Create Weapon Quality", parameters, options);
        var result = await dialog.Result;

        if (result is not null && !result.Canceled)
        {
            await LoadWeaponQualities();
        }
    }

    private async Task ShowEditDialog(Guid weaponQualityId)
    {
        var weaponQuality = weaponQualities?.FirstOrDefault(wq => wq.Id == weaponQualityId);
        if (weaponQuality != null)
        {
            var parameters = new DialogParameters
                {
                    { "WeaponQuality", weaponQuality }
                };

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, BackdropClick = false };

            var dialog = await DialogService.ShowAsync<WeaponQualityModal>("Edit Weapon Quality", parameters, options);
            var result = await dialog.Result;

            if (result is not null && !result.Canceled)
            {
                await LoadWeaponQualities();
            }
        }
    }


    private async Task DeleteWeaponQuality(Guid weaponQualityId)
    {
        var result = await _client.DeleteAsync($"/weapon-qualities/{weaponQualityId}");
        if (!result.IsSuccessStatusCode)
            Snackbar.Add("Error during deletion");
        else
            Snackbar.Add("Deleted");
        await LoadWeaponQualities();
    }
}