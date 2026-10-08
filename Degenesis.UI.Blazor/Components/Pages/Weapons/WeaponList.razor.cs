using Degenesis.Shared.DTOs;
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
    private string SearchString = "";

    protected override async Task OnAuthenticatedInitializedAsync()
    {
        await LoadWeapons();
    }

    private async Task LoadWeapons()
    {
        var weaponResult = await Client!.GetFromJsonAsync<Result<List<WeaponDto>>>("/weapons") ?? new Result<List<WeaponDto>> { IsError = true, Error = "Unknown error" };
        if (weaponResult.IsError)
        {
            Snackbar.Add($"Error loading weapons : {weaponResult.Error}", Severity.Error);
            Weapons = [];
        }
        else
            Weapons = weaponResult.Value ?? [];

        var weaponTypeResult = await Client!.GetFromJsonAsync<Result<List<WeaponTypeDto>>>("/weapon-types") ?? new Result<List<WeaponTypeDto>> { IsError = true, Error = "Unknown error" };
        if (weaponTypeResult.IsError)
        {
            Snackbar.Add($"Error loading weapon types: {weaponTypeResult.Error}", Severity.Error);
            WeaponTypes = [];
        }
        else
            WeaponTypes = weaponTypeResult.Value ?? [];

        var attributeResult = await Client!.GetFromJsonAsync<Result<List<AttributeDto>>>("/attributes") ?? new Result<List<AttributeDto>> { IsError = true, Error = "Unknown error" };
        if (attributeResult.IsError)
        {
            Snackbar.Add($"Error loading attributes: {attributeResult.Error}", Severity.Error);
            Attributes = [];
        }
        else
            Attributes = attributeResult.Value ?? [];

        var skillResult = await Client!.GetFromJsonAsync<Result<List<SkillDto>>>("/skills") ?? new Result<List<SkillDto>> { IsError = true, Error = "Unknown error" };
        if (skillResult.IsError)
        {
            Snackbar.Add($"Error loading skills: {skillResult.Error}", Severity.Error);
            Skills = [];
        }
        else
            Skills = skillResult.Value ?? [];

        var cultResult = await Client!.GetFromJsonAsync<Result<List<CultDto>>>("/cults") ?? new Result<List<CultDto>> { IsError = true, Error = "Unknown error" };
        if (cultResult.IsError)
        {
            Snackbar.Add($"Error loading cults: {cultResult.Error}", Severity.Error);
            Cults = [];
        }
        else
            Cults = cultResult.Value ?? [];
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
        var response = await Client!.DeleteAsync($"/weapons/{weaponId}");
        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result<object>>();
            Snackbar.Add(result?.Error ?? "Unknown error", Severity.Error);
        }
        else
            Snackbar.Add("Deleted", Severity.Success);
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

    private bool FilterFunc(WeaponDto weapon)
    {
        if (string.IsNullOrWhiteSpace(SearchString))
            return true;
        if (weapon.Name.Contains(SearchString, StringComparison.OrdinalIgnoreCase))
            return true;
        return false;
    }
}