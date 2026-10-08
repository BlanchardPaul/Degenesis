using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Characters.Display;
using Degenesis.Shared.DTOs.Weapons;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Rooms.CharacterInventory.Modals;

public partial class CharacterAddWeapon
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;
    [Parameter] public CharacterDisplayDto Character { get; set; } = new();
    public List<WeaponDto>? Weapons { get; set; }

    private string SearchString { get; set; } = "";

    protected override async Task OnAuthenticatedInitializedAsync()
    {
        var result = await Client!.GetFromJsonAsync<Result<List<WeaponDto>>>("/weapons") ?? new Result<List<WeaponDto>> {IsError = true, Error = "Unknown error" };
        if(result.IsError)
        {
            Snackbar.Add("Error loading weapons: " + result.Error);
            Weapons = [];
        }
        else
        {
            Weapons = result.Value;
        }
    }

    private async Task AddCharacterWeapon(Guid weaponId)
    {
        if (weaponId == Guid.Empty)
        {
            Snackbar.Add("Please select a weapon first.", Severity.Warning);
            return;
        }

        var response = await Client!.PostAsJsonAsync($"/character-weapons/", new { CharacterId = Character.Id, WeaponId = weaponId });
        if(response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result<object>>();
            Snackbar.Add(result?.Error ?? "Unknown error", Severity.Error);
            return;
        }
        else
            Snackbar.Add("Character weapon added successfully.", Severity.Success);

        MudDialog.Close(DialogResult.Ok(true));
    }

    private static string FormatDamage(WeaponDto weapon)
    {
        if (weapon.Damage == 0 && weapon.Attribute is null && weapon.Skill is null)
            return " - ";

        string damageString = string.Empty;

        if (weapon.Damage > 0) damageString += weapon.Damage.ToString();

        if (weapon.Attribute is not null || weapon.Skill is not null)
        {
            damageString += $" + (";
            if (weapon.Attribute is not null) damageString += $"{weapon.Attribute.Abbreviation}";
            if (weapon.Skill is not null)
            {
                if (weapon.Attribute is not null) damageString += " + ";
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

    private void Cancel() => MudDialog.Cancel();

    private bool FilterFunc(WeaponDto weapon)
    {
        if (string.IsNullOrWhiteSpace(SearchString))
            return true;
        if (weapon.Name.Contains(SearchString, StringComparison.OrdinalIgnoreCase))
            return true;
        return false;
    }
}
