using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Characters.Display;
using Degenesis.Shared.DTOs.Rooms;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Rooms;

public enum RollType
{
    Action,
    Free
}

public partial class DiceRollModal
{
    [Inject] private AuthenticationStateProvider AuthProvider { get; set; } = default!;
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;
    [Parameter] public CharacterDisplayDto? Character { get; set; }
    [Parameter] public HubConnection? HubConnection { get; set; }
    [Parameter] public Guid IdRoom { get; set; }
    private string UserName = string.Empty;
    public bool PrivateRoll { get; set; }

    // Action Roll
    private RollType _rollType = RollType.Action;
    private Guid? _selectedAttributeId;
    private Guid? _selectedSkillId;
    private int _bonus;
    private int _malus;
    private int _successes;
    private int _triggers;
    private int _botches;
    private int _failures;
    private int _autoSuccesses;
    private const int MaxActionDice = 12;

    // Free roll
    private int _diceCount = 1;
    private int _diceType = 6;
    private int _modifier = 0;
    private bool _isRolling;
    private List<int> _lastRolls = [];


    // Unicode dice faces for result display
    private static readonly string[] DiceFaces = [ "⚀", "⚁", "⚂", "⚃","⚄","⚅" ];

    protected override async Task OnInitializedAsync()
    {
        var auth = await AuthProvider.GetAuthenticationStateAsync();
        UserName = auth.User.FindFirst("unique_name")?.Value ?? string.Empty;
    }

    private static string GetDieColor(int value) => value switch
    {
        1 => "darkred",
        2 => "red",
        3 => "red",
        4 => "green",
        5 => "green",
        6 => "goldenrod",
        _ => ""
    };

    private CharacterAttributeDisplayDto? SelectedAttribute =>
        Character?.Attributes.FirstOrDefault(a => a.AttributeId == _selectedAttributeId);

    private CharacterSkillDisplayDto? SelectedSkill =>
        Character?.Skills.FirstOrDefault(s => s.SkillId == _selectedSkillId);

    private IEnumerable<CharacterSkillDisplayDto> SkillsOfSelectedAttribute =>
        Character?.Skills.Where(s => s.AttributeId == _selectedAttributeId) ?? [];

    private int ActionRollDiceCount
    {
        get
        {
            // Ensure that dice number is at least 1
            var baseDice = (SelectedAttribute?.Level ?? 0) + (SelectedSkill?.Level ?? 0);
            baseDice = Math.Max(baseDice + _bonus - _malus, 0);

            // Check if rolled dices > 12, if yes then dicesNumber-12 = auto successes
            _autoSuccesses = Math.Max(baseDice - MaxActionDice, 0);
            baseDice = Math.Min(baseDice, MaxActionDice);
            return baseDice;
        }
    }

    private async Task RollAsync()
    {
        if (HubConnection?.State != HubConnectionState.Connected)
        {
            Snackbar.Add("Chat connection unavailable, roll not sent.", Severity.Warning);
            _isRolling = false;
            return;
        }

        _isRolling = true;

        var random = new Random();
        var result = new DiceRollResultDto();
        if (_rollType == RollType.Action)
        {
            _lastRolls = [.. Enumerable.Range(0, Math.Max(ActionRollDiceCount, 1)).Select(_ => random.Next(1, 7))];
            _lastRolls.Sort();
            _failures = _lastRolls.Count(r => r is 2 or 3);
            _botches = _lastRolls.Count(r => r == 1);
            _successes = _lastRolls.Count(r => r is 4 or 5);
            _triggers = _lastRolls.Count(r => r == 6);

            // send to the chat
            result = new DiceRollResultDto
            {
                Roller = UserName ?? "?",
                RollDescription = $"{SelectedAttribute?.Name} {SelectedAttribute?.Level ?? 0} + {SelectedSkill?.Name} {SelectedSkill?.Level ?? 0}"
                      + (_bonus > 0 ? $" + {_bonus}" : "") + (_malus > 0 ? $" - {_malus}" : ""),
                DiceCount = _lastRolls.Count,
                Rolls = _lastRolls,
                Botches = _botches,
                Failures = _failures,
                Successes = _successes,
                Triggers = _triggers,
                Total = 0, 
                IsActionRoll = true,
                AutoSuccesses = _autoSuccesses
            };
        }
        else
        {
            _lastRolls = [.. Enumerable.Range(0, _diceCount).Select(_ => random.Next(1, _diceType + 1))];
            _lastRolls.Sort();
            _successes = _triggers = _failures = _botches = 0;

            result = new DiceRollResultDto
            {
                Roller = UserName,
                RollDescription = $"{_diceCount}d{_diceType}"
                      + (_modifier > 0 ? $" + {_modifier}" : _modifier < 0 ? $" - {Math.Abs(_modifier)}" : ""),
                DiceCount = _diceCount,
                Rolls = _lastRolls,
                Botches = 0,
                Failures = 0,
                Successes = 0,
                Triggers = 0,
                Total = _lastRolls.Sum() + _modifier,
                IsActionRoll = false,
                AutoSuccesses = 0
            };
        }
        if(!PrivateRoll)
            await HubConnection.SendAsync("SendRollResult", IdRoom.ToString(), result);
        _isRolling = false;
        StateHasChanged();
    }

    private void Cancel() => MudDialog.Cancel();

    private static string GetDieFace(int value) => DiceFaces[value - 1];

}