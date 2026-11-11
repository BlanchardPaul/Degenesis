using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Characters.Display;
using Degenesis.Shared.DTOs.Rooms;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Rooms.MapChatArea;

public partial class SavedRollsDrawer
{
    [Parameter] public CharacterDisplayDto? Character { get; set; }
    [Parameter] public HubConnection? HubConnection { get; set; }
    [Parameter] public EventCallback OnClose { get; set; }
    [Parameter] public Guid IdRoom { get; set; }
    [Parameter] public string? UserName { get; set; }

    private async Task RollSavedAsync(CharacterSavedDiceRollDisplayDto savedRoll)
    {
        if (Character is null || HubConnection is null)
            return;

        if (HubConnection.State != HubConnectionState.Connected)
        {
            Snackbar.Add("Chat connection unavailable, roll not sent.", Severity.Warning);
            return;
        }

        var attribute = Character.Attributes.FirstOrDefault(a => a.AttributeId == savedRoll.AttributeId);
        var skill = Character.Skills.FirstOrDefault(s => s.SkillId == savedRoll.SkillId);
        if (attribute is null || skill is null)
        {
            Snackbar.Add("Saved roll refers to an unknown attribute/skill.", Severity.Error);
            return;
        }

        const int maxActionDice = 12;
        int baseDice = attribute.Level + skill.Level;
        int autoSuccesses = Math.Max(baseDice - maxActionDice, 0);
        int diceCount = Math.Min(baseDice, maxActionDice);
        diceCount = Math.Max(diceCount, 1);

        var random = new Random();
        var rolls = Enumerable.Range(0, diceCount).Select(_ => random.Next(1, 7)).ToList();
        rolls.Sort();

        var result = new DiceRollResultDto
        {
            Roller = UserName ?? "?",
            RollDescription = $"{savedRoll.Name} ({attribute.Name} {attribute.Level} + {skill.Name} {skill.Level})",
            DiceCount = rolls.Count,
            Rolls = rolls,
            Botches = rolls.Count(r => r == 1),
            Failures = rolls.Count(r => r is 2 or 3),
            Successes = rolls.Count(r => r is 4 or 5),
            Triggers = rolls.Count(r => r == 6),
            Total = 0,
            IsActionRoll = true,
            AutoSuccesses = autoSuccesses
        };

        await HubConnection.SendAsync("SendRollResult", IdRoom.ToString(), result);
    }

    private async Task DeleteSavedRollAsync(Guid savedRollId)
    {
        var response = await Client!.DeleteAsync($"/character-saved-dice-rolls/{savedRollId}");
        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result<object>>();
            Snackbar.Add(result?.Error ?? "Unknown error", Severity.Error);
            return;
        }
        else
            Snackbar.Add("Deleted", Severity.Success);
    }
}
