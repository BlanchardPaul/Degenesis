using Degenesis.Shared.DTOs.Characters.CRUD;
using Degenesis.Shared.DTOs.Weapons;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Weapons;

public partial class WeaponList
{
    private List<WeaponDto>? Weapons;
    private List<WeaponTypeDto> WeaponTypes = [];
    private List<AttributeDto> Attributes = [];
    private List<SkillDto> Skills = [];
    private List<CultDto> Cults = [];
    private HttpClient _client = new();

    protected override async Task OnInitializedAsync()
    {
        _client = await HttpClientService.GetClientAsync();
        await LoadWeapons();
    }

    private async Task LoadWeapons()
    {
        Weapons = await _client.GetFromJsonAsync<List<WeaponDto>>("/weapons") ?? [];
        WeaponTypes = await _client.GetFromJsonAsync<List<WeaponTypeDto>>("/weapon-types") ?? [];
        Attributes = await _client.GetFromJsonAsync<List<AttributeDto>>("/attributes") ?? [];
        Skills = await _client.GetFromJsonAsync<List<SkillDto>>("/skills") ?? [];
        Cults = await _client.GetFromJsonAsync<List<CultDto>>("/cults") ?? [];
    }

    private async Task ShowCreateDialog()
    {
        var parameters = new DialogParameters
            {
                { "Weapon", new WeaponDto() },
                { "WeaponTypes", WeaponTypes },
                { "Attributes", Attributes },
                { "Skills", Skills },
                { "Cults", Cults }
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
        var weapon = Weapons?.FirstOrDefault(w => w.Id == weaponId);
        if (weapon != null)
        {
            var parameters = new DialogParameters
                {
                    { "Weapon", weapon },
                    { "WeaponTypes", WeaponTypes },
                    { "Attributes", Attributes },
                    { "Skills", Skills },
                    { "Cults", Cults }
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

    private static string FormatDamage(WeaponDto weapon)
    {
        if(weapon.Damage == 0 && weapon.Attribute is null && weapon.Skill is null)
            return " - ";

        string damageString = string.Empty;

        if (weapon.Damage > 0) damageString += weapon.Damage.ToString();

        if (weapon.Attribute is not null || weapon.Skill is not null)
        {
            damageString += $" + (";
            if(weapon.Attribute is not null) damageString += $"{weapon.Attribute.Abbreviation}";
            if(weapon.Skill is not null)
            {
                if(weapon.Attribute is not null) damageString += " + ";
                damageString += $"{weapon.Skill.Abbreviation}";
            }
            damageString += $")";
            if (weapon.CharacterAttributeModifier.HasValue && weapon.CharacterAttributeModifier > 0)
            {
                damageString += $"/{weapon.CharacterAttributeModifier}";
            }
        }

        return damageString;
    }
}