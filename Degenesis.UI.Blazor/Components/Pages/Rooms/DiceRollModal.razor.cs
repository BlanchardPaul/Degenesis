using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Characters.Display;
using Degenesis.Shared.DTOs.NPCs;
using Degenesis.Shared.DTOs.Rooms;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
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
    [Parameter] public Action? OnClose { get; set; }
    [Parameter] public CharacterDisplayDto? Character { get; set; }
    [Parameter] public HubConnection? HubConnection { get; set; }
    [Parameter] public Guid IdRoom { get; set; }
    [Parameter] public bool IsGM { get; set; }
    [Parameter] public List<CharacterDisplayForGMListDto> GmCharacters { get; set; } = [];
    [Parameter] public List<NPCDto> GmNpcs { get; set; } = [];

    private string UserName = string.Empty;
    public bool PrivateRoll { get; set; }

    // GM mode: selected character/NPC
    private Guid? _selectedCharacterId;
    private CharacterDisplayDto? _selectedCharacterData;
    private NPCDto? _selectedNpcData;

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
    private static readonly string[] DiceFaces = ["⚀", "⚁", "⚂", "⚃", "⚄", "⚅"];

    protected override async Task OnInitializedAsync()
    {
        var auth = await AuthProvider.GetAuthenticationStateAsync();
        UserName = auth.User.FindFirst("unique_name")?.Value ?? string.Empty;

        // If not GM, use the player's character
        if (!IsGM && Character is not null)
        {
            _selectedCharacterData = Character;
        }
    }

    private async Task HandleCharacterSelected(Guid? characterId)
    {
        if (characterId is null)
        {
            _selectedCharacterData = null;
            _selectedAttributeId = null;
            _selectedSkillId = null;
            _lastRolls.Clear();
            StateHasChanged();
            return;
        }

        _selectedCharacterId = characterId;

        _selectedAttributeId = null;
        _selectedSkillId = null;
        _lastRolls.Clear();

        // Check if it's a character or NPC
        var character = GmCharacters.FirstOrDefault(c => c.Id == characterId.Value);
        if (character is not null)
        {
            // Load full character data
            try
            {
                var response = await Client!.GetFromJsonAsync<Result<CharacterDisplayDto>>($"/characters/gm/character/{characterId}")
                    ?? new Result<CharacterDisplayDto> { IsError = true, Error = "Unknown error" };
                if (!response.IsError && response.Value is not null)
                {
                    _selectedCharacterData = response.Value;
                }
                else
                {
                    Snackbar.Add("Failed to load character", Severity.Error);
                }
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Error loading character: {ex.Message}", Severity.Error);
            }
        }
        else
        {
            // It's an NPC
            var npc = GmNpcs.FirstOrDefault(n => n.Id == characterId.Value);
            if (npc is not null)
            {
                _selectedCharacterData = ConvertNpcToCharacterDisplay(npc);
            }
        }

        StateHasChanged();
    }

    // Convertit un NPC en CharacterDisplayDto pour la réutilisabilité
    private CharacterDisplayDto ConvertNpcToCharacterDisplay(NPCDto npc)
    {
        return new CharacterDisplayDto
        {
            Id = npc.Id,
            Name = npc.Name,
            Attributes = [.. npc.Attributes.Select(a => new CharacterAttributeDisplayDto
            {
                AttributeId = a.AttributeId,
                Name = a.AttributeName,
                Level = a.Level
            })],
            Skills = [.. npc.Skills.Select(s => new CharacterSkillDisplayDto
            {
                SkillId = s.SkillId,
                Name = s.SkillName,
                Level = s.Level,
                AttributeId = s.AttributeId
            })]
        };
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
        _selectedCharacterData?.Attributes.FirstOrDefault(a => a.AttributeId == _selectedAttributeId);

    private CharacterSkillDisplayDto? SelectedSkill =>
        _selectedCharacterData?.Skills.FirstOrDefault(s => s.SkillId == _selectedSkillId);

    private IEnumerable<CharacterSkillDisplayDto> SkillsOfSelectedAttribute =>
        _selectedCharacterData?.Skills.Where(s => s.AttributeId == _selectedAttributeId) ?? [];

    private IEnumerable<CharacterAttributeDisplayDto> AvailableAttributes =>
        _selectedCharacterData?.Attributes ?? [];

    private int ActionRollDiceCount
    {
        get
        {
            var baseDice = (SelectedAttribute?.Level ?? 0) + (SelectedSkill?.Level ?? 0);
            baseDice = Math.Max(baseDice + _bonus - _malus, 0);

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
        if (!PrivateRoll)
        {
            if (IsGM && _selectedCharacterData is not null)
            {
                result.RolledForCharacterName = _selectedCharacterData.Name;
            }
            await HubConnection.SendAsync("SendRollResult", IdRoom.ToString(), result);
        }

        _isRolling = false;
        StateHasChanged();
    }

    private void Cancel() => OnClose?.Invoke();

    private static string GetDieFace(int value) => DiceFaces[value - 1];

    private async Task<IEnumerable<Guid?>> SearchCharacters(string value, CancellationToken cancellationToken = default)
    {
        await Task.Delay(0, cancellationToken);
        if (string.IsNullOrWhiteSpace(value))
        {
            var allIds = GmCharacters.Select(c => (Guid?)c.Id).ToList();
            allIds.AddRange(GmNpcs.Select(n => (Guid?)n.Id));
            return allIds;
        }

        var searchLower = value.ToLower();
        var result = new List<Guid?>();

        result.AddRange(GmCharacters
            .Where(c => c.Name.ToLower().Contains(searchLower) || c.PlayerName.ToLower().Contains(searchLower))
            .Select(c => (Guid?)c.Id));

        result.AddRange(GmNpcs
            .Where(n => n.Name.ToLower().Contains(searchLower))
            .Select(n => (Guid?)n.Id));

        return result;
    }

    private string GetCharacterName(Guid? id)
    {
        if (id is null)
            return "";

        var character = GmCharacters.FirstOrDefault(c => c.Id == id.Value);
        if (character is not null)
            return $"{character.Name} ({character.PlayerName})";

        var npc = GmNpcs.FirstOrDefault(n => n.Id == id.Value);
        return npc?.Name ?? "";
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            try
            {
                await JS.InvokeAsync<object>("floatingWindow.enableDragging", "dice-roll-window");
            }
            catch { }
        }
        await base.OnAfterRenderAsync(firstRender);
    }

}