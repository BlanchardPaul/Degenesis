using Degenesis.Shared.DTOs.Characters.CRUD;
using Degenesis.Shared.DTOs.Characters.Display;
using Degenesis.UI.Blazor.Components.Pages.Rooms.CharactersSheetModals;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Rooms;

public partial class RoomCharacterSheet
{
    [Parameter] public Guid? IdRoom { get; set; }
    [Parameter] public CharacterDisplayDto? Character { get; set; }
    [Parameter] public EventCallback OnRequestParentReload { get; set; }
    private HttpClient _client = new();

    protected override async Task OnInitializedAsync()
    {
        _client = await HttpClientService.GetClientAsync();
    }

    private bool IsSkillDisabled(CharacterSkillDisplayDto characterSkill)
    {
        if(Character is null)
            return false;

        if (characterSkill.Name == "PRIMAL" && Character.IsFocusOriented)
            return true;

        if (characterSkill.Name == "FOCUS" && !Character.IsFocusOriented)
            return true;

        if (characterSkill.Name == "FAITH" && Character.Skills.First(s => s.Name == "WILLPOWER").Level > 0)
            return true;

        if (characterSkill.Name == "WILLPOWER" && Character.Skills.First(s => s.Name == "FAITH").Level > 0)
            return true;

        return false;
    }

    private async Task OpenEditInfosDialog()
    {
        if (Character is null)
        {
            Snackbar.Add("No character loaded.", Severity.Warning);
            return;
        }
        var parameters = new DialogParameters
            {
                { "CharacterInfos", new CharacterBasicInfosEditDto
                    {
                        Id = Character.Id,
                        Name = Character.Name,
                        Age = Character.Age,
                        Height = Character.Height,
                        Weight = Character.Weight,
                        Sex = Character.Sex,
                        IsFocusOriented = Character.IsFocusOriented
                    }
                }
            };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, BackdropClick = false };

        var dialog = await DialogService.ShowAsync<CharacterEditInfosModal>("Edit Basic infos", parameters, options);
        var result = await dialog.Result;

        if (result is not null && !result.Canceled)
        {
            Snackbar.Add("Character infos updated", Severity.Success);
            await OnRequestParentReload.InvokeAsync();
            StateHasChanged();
        }
    }

    private async Task OpenEditRankDialog()
    {
        if (Character is null)
        {
            Snackbar.Add("No character loaded.", Severity.Warning);
            return;
        }

        var parameters = new DialogParameters { { "Character", Character } };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, BackdropClick = false };

        var dialog = await DialogService.ShowAsync<CharacterEditRankModal>("Select a new rank", parameters, options);
        var result = await dialog.Result;

        if (result is not null && !result.Canceled)
        {
            await OnRequestParentReload.InvokeAsync();
            StateHasChanged();
        }

    }

    private async Task OpenAddPotentialDialog()
    {
        if (Character is null)
        {
            Snackbar.Add("No character loaded.", Severity.Warning);
            return;
        }

        var parameters = new DialogParameters { { "Character", Character } };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, BackdropClick = false };

        var dialog = await DialogService.ShowAsync<CharacterAddPotentialModal>("Add a potential", parameters, options);
        var result = await dialog.Result;

        if (result is not null && !result.Canceled)
        {
            await OnRequestParentReload.InvokeAsync();
            StateHasChanged();
        }
    }

    private async Task UpdateXp()
    {
        if (Character is null)
            return;

        var result = await _client.PutAsJsonAsync($"/characters/xp/", new CharacterIntValueEditDto {Id = Character.Id, Value = Character.Experience });

        if (!result.IsSuccessStatusCode)
        {
            Snackbar.Add("Error while updating XP", Severity.Error);
        }
    }

    private async Task UpdateChroniclerMoney()
    {
        if (Character is null)
            return;

        var result = await _client.PutAsJsonAsync($"/characters/chroniclermoney/", new CharacterIntValueEditDto { Id = Character.Id, Value = Character.ChroniclerMoney });

        if (!result.IsSuccessStatusCode)
        {
            Snackbar.Add("Error while updating Chronicler money", Severity.Error);
        }
    }

    private async Task UpdateDinar()
    {
        if (Character is null)
            return;

        var result = await _client.PutAsJsonAsync($"/characters/dinar/", new CharacterIntValueEditDto { Id = Character.Id, Value = Character.DinarMoney });

        if (!result.IsSuccessStatusCode)
        {
            Snackbar.Add("Error while updating Dinars", Severity.Error);
        }
    }

    private async Task UpdatePermanentSporeInfestation()
    {
        if (Character is null)
            return;

        var result = await _client.PutAsJsonAsync($"/characters/permanent-spore-infestation/", new CharacterIntValueEditDto { Id = Character.Id, Value = Character.PermanentSporeInfestation });

        if (!result.IsSuccessStatusCode)
        {
            Snackbar.Add("Error while updating Permanent Spore Infestation", Severity.Error);
        }
    }
    
    private async Task UpdateEgo(int newValue)
    {
        if (Character is null)
            return;

        Character.Ego = newValue;

        var result = await _client.PutAsJsonAsync($"/characters/ego/", new CharacterIntValueEditDto { Id = Character.Id, Value = Character.Ego });

        if (!result.IsSuccessStatusCode)
        {
            Snackbar.Add("Error while updating Ego", Severity.Error);
        }
    }

    private async Task UpdateCurrentSporeInfestation(int newValue)
    {
        if (Character is null)
            return;

        Character.CurrentSporeInfestation = newValue;

        var result = await _client.PutAsJsonAsync($"/characters/current-spore-infestation/", new CharacterIntValueEditDto { Id = Character.Id, Value = Character.CurrentSporeInfestation });

        if (!result.IsSuccessStatusCode)
        {
            Snackbar.Add("Error while updating Current Spore Infestation", Severity.Error);
        }
    }

    private async Task UpdateFleshWounds(int newValue)
    {
        if (Character is null)
            return;

        Character.FleshWounds = newValue;

        var result = await _client.PutAsJsonAsync($"/characters/fleshwounds/", new CharacterIntValueEditDto { Id = Character.Id, Value = Character.FleshWounds });

        if (!result.IsSuccessStatusCode)
        {
            Snackbar.Add("Error while updating FleshWounds", Severity.Error);
        }
    }

    private async Task UpdateTrauma(int newValue)
    {
        if (Character is null)
            return;

        Character.Trauma = newValue;

        var result = await _client.PutAsJsonAsync($"/characters/trauma/", new CharacterIntValueEditDto { Id = Character.Id, Value = Character.Trauma });

        if (!result.IsSuccessStatusCode)
        {
            Snackbar.Add("Error while updating Trauma", Severity.Error);
        }
    }

    private async Task UpdateBackgroundLevel(CharacterBackgroundDisplayDto background, int newValue)
    {
        if (Character is null)
            return;

        background.Level = newValue;

        var updateDto = new CharacterBackgroundDto
        {
            CharacterId = Character.Id,
            BackgroundId = background.BackgroundId,
            Level = newValue
        };

        var result = await _client.PutAsJsonAsync("/character-backgrounds/", updateDto);

        if (!result.IsSuccessStatusCode)
        {
            Snackbar.Add($"Error while updating {background.Name}", Severity.Error);
        }
    }

    private async Task ChangeAttributeLevel(CharacterAttributeDisplayDto attribute, int direction)
    {
        if (Character is null)
            return;

        int oldValue = attribute.Level;
        int newValue = oldValue + direction;

        if (direction < 0)
        {
            int refund = CalculateXpCostForAttribute(Character, attribute, newValue, oldValue);
            Character.Experience += refund;
            attribute.Level = newValue;

            Snackbar.Add($"{attribute.Name} decreased to {newValue}. Refunded {refund} XP.", Severity.Info);
            
            await UpdateXp();
            await UpdateCharacterAttribute(attribute);

            StateHasChanged();
            return;
        }

        int cost = CalculateXpCostForAttribute(Character, attribute, oldValue, newValue);
        if (Character.Experience < cost)
        {
            Snackbar.Add($"Not enough XP! Needed: {cost}, Available: {Character.Experience}", Severity.Error);
            return;
        }

        Character.Experience -= cost;
        attribute.Level = newValue;

        Snackbar.Add($"{attribute.Name} increased to {newValue} (Cost {cost} XP)", Severity.Success);

        await UpdateXp();
        await UpdateCharacterAttribute(attribute);

        StateHasChanged();
    }

    private async Task ChangeSkillLevel(CharacterSkillDisplayDto skill, int direction)
    {
        if (Character is null)
            return;

        int oldValue = skill.Level;
        int newValue = oldValue + direction;

        if (direction < 0)
        {
            int refund = CalculateXpCostForSkill(Character, skill, newValue, oldValue);
            Character.Experience += refund;
            skill.Level = newValue;

            Snackbar.Add($"{skill.Name} decreased to {newValue}. Refunded {refund} XP.", Severity.Info);

            await UpdateXp();
            await UpdateCharacterSkill(skill);

            StateHasChanged();
            return;
        }

        int cost = CalculateXpCostForSkill(Character, skill, oldValue, newValue);
        if (Character.Experience < cost)
        {
            Snackbar.Add($"Not enough XP! Needed: {cost}, Available: {Character.Experience}", Severity.Error);
            return;
        }

        Character.Experience -= cost;
        skill.Level = newValue;

        Snackbar.Add($"{skill.Name} increased to {newValue} (Cost {cost} XP)", Severity.Success);

        await UpdateXp();
        await UpdateCharacterSkill(skill);

        StateHasChanged();
    }

    private async Task UpdateCharacterAttribute(CharacterAttributeDisplayDto attribute)
    {
        var updateDto = new CharacterAttributeDto
        {
            CharacterId = Character!.Id,
            AttributeId = attribute.AttributeId,
            Level = attribute.Level
        };

        var result = await _client.PutAsJsonAsync("/character-attributes/", updateDto);

        if (!result.IsSuccessStatusCode)
            Snackbar.Add($"Error while updating {attribute.Name}", Severity.Error);
        else
        {
            await OnRequestParentReload.InvokeAsync();
            StateHasChanged();
        }
    }

    private async Task UpdateCharacterSkill(CharacterSkillDisplayDto skill)
    {
        var updateDto = new CharacterSkillDto
        {
            CharacterId = Character!.Id,
            SkillId = skill.SkillId,
            Level = skill.Level
        };

        var result = await _client.PutAsJsonAsync("/character-skills/", updateDto);

        if (!result.IsSuccessStatusCode)
            Snackbar.Add($"Error while updating {skill.Name}", Severity.Error);
        else
        {
            await OnRequestParentReload.InvokeAsync();
            StateHasChanged();
        }
    }

    private async Task UpdateCharacterPotential(CharacterPotentialDisplayDto potential)
    {
        var updateDto = new CharacterPotentialDto
        {
            CharacterId = Character!.Id,
            PotentialId = potential.PotentialId,
            Level = potential.Level
        };

        var result = await _client.PutAsJsonAsync("/character-potentials/", updateDto);

        if (!result.IsSuccessStatusCode)
            Snackbar.Add($"Error while updating {potential.Name}", Severity.Error);
        else
        {
            await OnRequestParentReload.InvokeAsync();
            StateHasChanged();
        }
    }

    private async Task ChangeBackgroundLevel(CharacterPotentialDisplayDto potential, int direction)
    {
        if (Character is null)
            return;

        int oldValue = potential.Level;
        int newValue = oldValue + direction;

        if (direction < 0)
        {
            int refund = CalculateXpCostForPotential(Character, potential, oldValue, newValue);
            Character.Experience += refund;
            potential.Level = newValue;

            Snackbar.Add($"{potential.Name} decreased to {newValue}. Refunded {refund} XP.", Severity.Info);

            await UpdateXp();
            await UpdateCharacterPotential(potential);

            StateHasChanged();
            return;
        }

        int cost = CalculateXpCostForPotential(Character, potential, oldValue, newValue);
        if (Character.Experience < cost)
        {
            Snackbar.Add($"Not enough XP! Needed: {cost}, Available: {Character.Experience}", Severity.Error);
            return;
        }

        Character.Experience -= cost;
        potential.Level = newValue;

        Snackbar.Add($"{potential.Name} increased to {newValue} (Cost {cost} XP)", Severity.Success);

        await UpdateXp();
        await UpdateCharacterPotential(potential);

        StateHasChanged();
    }

    private static int CalculateXpCostForAttribute(CharacterDisplayDto character, CharacterAttributeDisplayDto attribute, int oldValue, int newValue)
    {
        bool isPreferred =
            (character.IsFocusOriented && new[] { "INTELLECT", "AGILITY", "PSYCHE" }.Contains(attribute.Name.ToUpper())) ||
            (!character.IsFocusOriented && new[] { "BODY", "CHARISMA", "INSTINCT" }.Contains(attribute.Name.ToUpper()));

        int factor = isPreferred ? 10 : 12;
        int level = newValue > oldValue ? newValue : oldValue;
        return level * factor;
    }

    private static int CalculateXpCostForSkill(CharacterDisplayDto character, CharacterSkillDisplayDto skill, int oldValue, int newValue)
    {
        var attribute = character.Attributes.FirstOrDefault(a => a.AttributeId == skill.AttributeId);
        bool isPreferred = attribute is not null &&
            ((character.IsFocusOriented && new[] { "INTELLECT", "AGILITY", "PSYCHE" }.Contains(attribute.Name.ToUpper())) ||
             (!character.IsFocusOriented && new[] { "BODY", "CHARISMA", "INSTINCT" }.Contains(attribute.Name.ToUpper())));

        int factor = isPreferred ? 4 : 5;
        int level = newValue > oldValue ? newValue : oldValue;
        return level * factor;
    }

    private static int CalculateXpCostForPotential(CharacterDisplayDto character, CharacterPotentialDisplayDto characterPotential, int oldValue, int newValue)
    {
        int totalPotentialLevels = character.Potentials.Sum(p => p.Level);
        int cost = newValue > oldValue ? totalPotentialLevels+1 : totalPotentialLevels ;
        return cost * 10;
    }

    private async Task DeleteCharacterPotential(Guid characterPotentialId)
    {
        if (Character is null)
            return;

        var result = await _client.DeleteAsync($"/character-potentials/{Character.Id}/{characterPotentialId}");

        if (!result.IsSuccessStatusCode)
        {
            Snackbar.Add("Error while deleting Character Potential", Severity.Error);
        }
        else
        {
            await OnRequestParentReload.InvokeAsync();
            StateHasChanged();
        }
    }
}
