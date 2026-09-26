using Degenesis.Shared.DTOs.Weapons;
using Microsoft.AspNetCore.Components;

namespace Degenesis.UI.Blazor.Components.Pages.Rooms.CharacterInventory.Popovers;

public partial class WeaponPopover
{
    [Parameter]
    public WeaponDto Weapon { get; set; } = null!;

    [Parameter]
    public EventCallback OnClose { get; set; }

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

    private async Task Close()
    {
        await OnClose.InvokeAsync();
    }
}
