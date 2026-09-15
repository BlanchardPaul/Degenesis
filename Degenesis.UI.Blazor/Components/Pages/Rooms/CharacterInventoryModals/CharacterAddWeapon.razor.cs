using Degenesis.Shared.DTOs.Characters.Display;
using Degenesis.Shared.DTOs.Weapons;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Rooms.CharacterInventoryModals;

public partial class CharacterAddWeapon
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;
    [Parameter] public CharacterDisplayDto Character { get; set; } = new();
    public List<WeaponDto> Weapons { get; set; } = [];

    private HttpClient _client = new();
    private bool WeaponsLoaded;

    protected override async Task OnInitializedAsync()
    {
        _client = await HttpClientService.GetClientAsync();
        Weapons = await _client.GetFromJsonAsync<List<WeaponDto>>("/weapons") ?? [];
        WeaponsLoaded = true;
    }

    private async Task AddCharacterWeapon(Guid weaponId)
    {
        if (weaponId == Guid.Empty)
        {
            Snackbar.Add("Please select a weapon first.", Severity.Warning);
            return;
        }
        var result = await _client.PostAsJsonAsync($"/character-weapons/", new { CharacterId = Character.Id, WeaponId = weaponId });
        if (!result.IsSuccessStatusCode)
            Snackbar.Add("Error while adding character weapon", Severity.Error);
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
}
