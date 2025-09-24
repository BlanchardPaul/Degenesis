using Degenesis.Shared.DTOs.Characters;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Characters;

public partial class CreateCharacter
{
    [Parameter] public Guid RoomId { get; set; }
    private HttpClient _client = new();

    private CharacterCreateDto Character = new();
    private List<CultDto> AvailableCults = [];
    private List<CultureDto> Cultures = [];
    private List<ConceptDto> Concepts = [];

    private int _step = 0;
    private bool _saving = false;

    private List<AttributeDto> Attributes = [];
    private List<SkillDto> Skills = [];

    private List<CharacterAttributeDto> CharacterAttributes = [];
    private List<CharacterSkillDto> CharacterSkills = [];
    private List<CharacterBackgroundDto> CharacterBackgrounds = [];

    private CultureDto SelectedCulture = new();
    private CultDto SelectedCult = new();
    private ConceptDto selectedConcept = new();
    private CharacterBasicInfoStep _basicInfoStep = default!;
    private CharacterStatsStep _characterStatsStep = default!;

    private CharacterBackgroundStep _characterBackgroundStep = default!;
    private List<BackgroundDto> Backgrounds = [];

    private List<RankDto> SortedRanks = [];
    private List<RankDto> Ranks = [];
    private CharacterRankStep _rankStep = default!;

    protected override async Task OnInitializedAsync()
    {
        Character.IdRoom = RoomId;
        _client = await HttpClientService.GetClientAsync();
        Cultures = await _client.GetFromJsonAsync<List<CultureDto>>("/cultures") ?? [];
        Concepts = await _client.GetFromJsonAsync<List<ConceptDto>>("/concepts") ?? [];
        Attributes = await _client.GetFromJsonAsync<List<AttributeDto>>("/attributes") ?? [];
        Skills = await _client.GetFromJsonAsync<List<SkillDto>>("/skills") ?? [];
        Backgrounds = await _client.GetFromJsonAsync<List<BackgroundDto>>("/backgrounds") ?? [];
        Ranks = await _client.GetFromJsonAsync<List<RankDto>>("/ranks") ?? [];
    }

    // Those are step 1 logic but it's here because we need some of the step 1 DTOs in step 2
    private Task OnCultureChanged(Guid selectedCultureId)
    {
        Character.CultureId = selectedCultureId;
        SelectedCulture = Cultures.FirstOrDefault(c => c.Id == selectedCultureId) ?? new();

        AvailableCults = SelectedCulture.AvailableCults ?? [];
        Character.CultId = Guid.Empty;
        return Task.CompletedTask;
    }

    private Task OnCultChanged(Guid selectedCultId)
    {
        Character.CultId = selectedCultId;
        SelectedCult = AvailableCults.FirstOrDefault(c => c.Id == selectedCultId) ?? new();
        SortedRanks = Ranks.Where(r => r.CultId == Character.CultId).ToList();
        Character.RankId = Guid.Empty;
        return Task.CompletedTask;
    }

    private Task OnConceptChanged(Guid selectedConceptId)
    {
        Character.ConceptId = selectedConceptId;
        selectedConcept = Concepts.FirstOrDefault(c => c.Id == selectedConceptId) ?? new();
        return Task.CompletedTask;
    }

    // OnPreviewInteraction intercepts preview actions (Complete, Activate, ...)
    private async Task OnPreviewInteraction(StepperInteractionEventArgs arg)
    {
        // When user clicks the "Complete" button, arg.Action == StepAction.Complete
        if (arg.Action == StepAction.Complete)
        {
            // If the user completes the final step (index 3 here), save the character
            const int finalStepIndex = 4;
            if (arg.StepIndex == finalStepIndex)
            {
                // Prevent double submission
                if (_saving)
                {
                    arg.Cancel = true;
                    return;
                }

                _saving = true;
                try
                {
                    var success = await SaveCharacterAsync();
                    if (!success)
                    {
                        // cancel stepper action so user remains on finish step
                        arg.Cancel = true;
                    }
                }
                finally
                {
                    _saving = false;
                }
            }
        }
    }

    private async Task<bool> SaveCharacterAsync()
    {
        try
        {
            var response = await _client.PostAsJsonAsync("/characters", Character);
            if (response.IsSuccessStatusCode)
            {
                Snackbar.Add("Character successfully created!", Severity.Success);
                Navigation.NavigateTo("/rooms");
                return true;
            }
            else
            {
                var msg = await response.Content.ReadAsStringAsync();
                Snackbar.Add($"Failed to save character: {response.StatusCode} - {msg}", Severity.Error);
                return false;
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error while saving character: {ex.Message}", Severity.Error);
            return false;
        }
    }

    // Forms validation logic is in their respective .razor.cs, here we call them to allow
    // the next step access
    #region steps validation
    private async Task ValidateStepBasic()
    {
        if (await _basicInfoStep.ValidateFormAsync())
            _step = 1;
    }

    private async Task ValidateStepStats()
    {
        if (await _characterStatsStep.ValidateFormAsync())
        {
            // Retrieve attributes and skills from step 2 if valid
            Character.Attributes = _characterStatsStep.CharacterAttributes;
            Character.Skills = _characterStatsStep.CharacterSkills;
            Character.MaxEgo = _characterStatsStep.MaxEgo;
            Character.MaxSporeInfestation = _characterStatsStep.MaxSporeInfestation;
            Character.MaxFleshWounds = _characterStatsStep.MaxFleshWounds;
            Character.MaxTrauma = _characterStatsStep.MaxTrauma;
            _step = 2;
        }
    }

    private async Task ValidateStepBackgrounds()
    {
        if (await _characterBackgroundStep.ValidateFormAsync())
        {
            Character.Backgrounds = _characterBackgroundStep.CharacterBackgrounds;
            _step = 3;
        }
    }

    private async Task ValidateStepRank()
    {
        if (await _rankStep.ValidateFormAsync())
        {
            _step = 4;
        }
    }
    #endregion
}
