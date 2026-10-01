using Degenesis.Shared.DTOs.Burns;
using Degenesis.Shared.DTOs.Characters.CRUD;
using Degenesis.Shared.DTOs.Characters.CRUD.Inventory;
using Degenesis.Shared.DTOs.Characters.Display;
using Degenesis.Shared.DTOs.Equipments;
using Degenesis.Shared.DTOs.Protections;
using Degenesis.Shared.DTOs.Vehicles;
using Degenesis.Shared.DTOs.Weapons;
using Degenesis.UI.Blazor.Components.Pages.Rooms.CharacterInventory.Modals;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Rooms;

public partial class RoomCharacterInventory
{
    [Parameter] public CharacterDisplayDto? Character { get; set; }
    [Parameter] public EventCallback OnRequestParentReload { get; set; }

    // Artifacts
    bool ExpandedArtifact = true;
    private async Task OpenAddCharacterArtifactDialog()
    {
        if (Character is null)
        {
            Snackbar.Add("No character loaded.", Severity.Warning);
            return;
        }

        var parameters = new DialogParameters { { "Character", Character } };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.ExtraLarge, FullWidth = true, BackdropClick = false };

        var dialog = await DialogService.ShowAsync<CharacterAddArtifact>("Add an artifact", parameters, options);
        var result = await dialog.Result;

        if (result is not null && !result.Canceled)
        {
            await OnRequestParentReload.InvokeAsync();
            StateHasChanged();
        }
    }

    private async Task UpdateCharacterArtifact(CharacterArtifactDto CharacterArtifact)
    {
        if (Character is null)
            return;

        var result = await Client!.PutAsJsonAsync($"/character-artifacts/", CharacterArtifact);

        if (!result.IsSuccessStatusCode)
        {
            Snackbar.Add("Error while updating Artifact", Severity.Error);
        }
    }

    private async Task DeleteCharacterArtifact(Guid artifactId)
    {
        var result = await Client!.DeleteAsync($"/character-artifacts/{artifactId}");
        if (!result.IsSuccessStatusCode)
            Snackbar.Add("Error during deletion", Severity.Error);
        else
        {
            await OnRequestParentReload.InvokeAsync();
            StateHasChanged();
        }
    }

    // Burns
    private BurnDto? SelectedBurn { get; set; }
    bool ExpandedBurn = true;
    private async Task OpenAddCharacterBurnDialog()
    {
        if (Character is null)
        {
            Snackbar.Add("No character loaded.", Severity.Warning);
            return;
        }

        var parameters = new DialogParameters { { "Character", Character } };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.ExtraLarge, FullWidth = true, BackdropClick = false };

        var dialog = await DialogService.ShowAsync<CharacterAddBurn>("Add an artifact", parameters, options);
        var result = await dialog.Result;

        if (result is not null && !result.Canceled)
        {
            await OnRequestParentReload.InvokeAsync();
            StateHasChanged();
        }
    }

    private async Task UpdateCharacterBurn(CharacterBurnDto CharacterBurn)
    {
        if (Character is null)
            return;
        var result = await Client!.PutAsJsonAsync($"/character-burns/", CharacterBurn);
        if (!result.IsSuccessStatusCode)
        {
            Snackbar.Add("Error while updating Burn", Severity.Error);
        }
    }

    private async Task DeleteCharacterBurn(Guid burnId)
    {
        var result = await Client!.DeleteAsync($"/character-burns/{burnId}");
        if (!result.IsSuccessStatusCode)
            Snackbar.Add("Error during deletion", Severity.Error);
        else
        {
            await OnRequestParentReload.InvokeAsync();
            StateHasChanged();
        }
    }
    private void OpenBurnPopover(BurnDto burn)
    {
        SelectedBurn = burn;
    }

    private void CloseBurnPopover()
    {
        SelectedBurn = null;
    }

    // Equipments
    private EquipmentDto? SelectedEquipment { get; set; }
    bool ExpandedEquipment = true;
    private async Task OpenAddCharacterEquipmentDialog()
    {
        if (Character is null)
        {
            Snackbar.Add("No character loaded.", Severity.Warning);
            return;
        }
        var parameters = new DialogParameters { { "Character", Character } };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.ExtraLarge, FullWidth = true, BackdropClick = false };
        var dialog = await DialogService.ShowAsync<CharacterAddEquipment>("Add an equipment", parameters, options);
        var result = await dialog.Result;
        if (result is not null && !result.Canceled)
        {
            await OnRequestParentReload.InvokeAsync();
            StateHasChanged();
        }
    }

    private async Task DeleteCharacterEquipment(Guid equipmentId)
    {
        var result = await Client!.DeleteAsync($"/character-equipments/{equipmentId}");
        if (!result.IsSuccessStatusCode)
            Snackbar.Add("Error during deletion", Severity.Error);
        else
        {
            await OnRequestParentReload.InvokeAsync();
            StateHasChanged();
        }
    }

    private void OpenEquipmentPopover(EquipmentDto equipment)
    {
        SelectedEquipment = equipment;
    }

    private void CloseEquipmentPopover()
    {
        SelectedEquipment = null;
    }

    // Protections
    private ProtectionDto? SelectedProtection { get; set; }
    bool ExpandedProtection = true;
    private async Task OpenAddCharacterProtectionDialog()
    {
        if (Character is null)
        {
            Snackbar.Add("No character loaded.", Severity.Warning);
            return;
        }
        var parameters = new DialogParameters { { "Character", Character } };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.ExtraLarge, FullWidth = true, BackdropClick = false };
        var dialog = await DialogService.ShowAsync<CharacterAddProtection>("Add a protection", parameters, options);
        var result = await dialog.Result;
        if (result is not null && !result.Canceled)
        {
            await OnRequestParentReload.InvokeAsync();
            StateHasChanged();
        }
    }

    private async Task UpdateCharacterProtection(CharacterProtectionDto CharacterProtection)
    {
        if (Character is null)
            return;
        var result = await Client!.PutAsJsonAsync($"/character-protections/", CharacterProtection);
        if (!result.IsSuccessStatusCode)
        {
            Snackbar.Add("Error while updating Protection", Severity.Error);
        }
    }

    private async Task DeleteCharacterProtection(Guid protectionId)
    {
        var result = await Client!.DeleteAsync($"/character-protections/{protectionId}");
        if (!result.IsSuccessStatusCode)
            Snackbar.Add("Error during deletion", Severity.Error);
        else
        {
            await OnRequestParentReload.InvokeAsync();
            StateHasChanged();
        }
    }

    private void OpenProtectionPopover(ProtectionDto equipment)
    {
        SelectedProtection = equipment;
    }

    private void CloseProtectionPopover()
    {
        SelectedProtection = null;
    }

    // Vehicles
    private VehicleDto? SelectedVehicle { get; set; }
    bool ExpandedVehicle = true;
    private async Task OpenAddCharacterVehicleDialog()
    {
        if (Character is null)
        {
            Snackbar.Add("No character loaded.", Severity.Warning);
            return;
        }
        var parameters = new DialogParameters { { "Character", Character } };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.ExtraLarge, FullWidth = true, BackdropClick = false };
        var dialog = await DialogService.ShowAsync<CharacterAddVehicle>("Add a vehicle", parameters, options);
        var result = await dialog.Result;
        if (result is not null && !result.Canceled)
        {
            await OnRequestParentReload.InvokeAsync();
            StateHasChanged();
        }
    }

    private async Task UpdateCharacterVehicle(CharacterVehicleDto CharacterVehicle)
    {
        if (Character is null)
            return;
        var result = await Client!.PutAsJsonAsync($"/character-vehicles/", CharacterVehicle);
        if (!result.IsSuccessStatusCode)
        {
            Snackbar.Add("Error while updating Vehicle", Severity.Error);
        }
    }

    private async Task DeleteCharacterVehicle(Guid vehicleId)
    {
        var result = await Client!.DeleteAsync($"/character-vehicles/{vehicleId }");
        if (!result.IsSuccessStatusCode)
            Snackbar.Add("Error during deletion", Severity.Error);
        else
        {
            await OnRequestParentReload.InvokeAsync();
            StateHasChanged();
        }
    }

    private void OpenVehiclePopover(VehicleDto vehicleDto)
    {
        SelectedVehicle = vehicleDto;
    }

    private void CloseVehiclePopover()
    {
        SelectedVehicle = null;
    }

    // Weapons
    private WeaponDto? SelectedWeapon { get; set; }
    bool ExpandedWeapon = true;
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

    private async Task OpenAddCharacterWeaponDialog()
    {
        if (Character is null)
        {
            Snackbar.Add("No character loaded.", Severity.Warning);
            return;
        }
        var parameters = new DialogParameters { { "Character", Character } };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.ExtraLarge, FullWidth = true, BackdropClick = false };
        var dialog = await DialogService.ShowAsync<CharacterAddWeapon>("Add a weapon", parameters, options);
        var result = await dialog.Result;
        if (result is not null && !result.Canceled)
        {
            await OnRequestParentReload.InvokeAsync();
            StateHasChanged();
        }
    }

    private async Task UpdateCharacterWeapon(CharacterWeaponDto CharacterWeapon)
    {
        if (Character is null)
            return;
        var result = await Client!.PutAsJsonAsync($"/character-weapons/", CharacterWeapon);
        if (!result.IsSuccessStatusCode)
        {
            Snackbar.Add("Error while updating Weapon", Severity.Error);
        }
    }

    private async Task DeleteCharacterWeapon(Guid weaponId)
    {
        var result = await Client!.DeleteAsync($"/character-weapons/{weaponId}");
        if (!result.IsSuccessStatusCode)
            Snackbar.Add("Error during deletion", Severity.Error);
        else
        {
            await OnRequestParentReload.InvokeAsync();
            StateHasChanged();
        }
    }

    private void OpenWeaponPopover(WeaponDto weaponDto)
    {
        SelectedWeapon = weaponDto;
    }

    private void CloseWeaponPopover()
    {
        SelectedWeapon = null;
    }


    // Free Notes area

    private async Task UpdateCharacterInventoryNotes()
    {
        if (Character is null)
            return;

        var result = await Client!.PutAsJsonAsync($"/characters/inventory-notes/", new CharacterStringValueEditDto { Id = Character.Id, Value = Character.InventoryNotes });

        if (!result.IsSuccessStatusCode)
        {
            Snackbar.Add("Error while updating inventory notes", Severity.Error);
        }
    }
}
