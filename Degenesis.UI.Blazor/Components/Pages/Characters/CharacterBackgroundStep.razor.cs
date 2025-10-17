using Degenesis.Shared.DTOs.Characters.CRUD;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Characters;

public partial class CharacterBackgroundStep
{
    [Parameter] public List<BackgroundDto> Backgrounds { get; set; } = [];
    [Parameter] public List<CharacterBackgroundDto> CharacterBackgrounds { get; set; } = [];

    private MudForm _formStepBackgrounds = default!;

    private const int MaxTotalPoints = 4;

    protected override void OnParametersSet()
    {
        foreach (var bg in Backgrounds)
        {
            if (!CharacterBackgrounds.Any(b => b.BackgroundId == bg.Id))
            {
                CharacterBackgrounds.Add(new CharacterBackgroundDto
                {
                    BackgroundId = bg.Id,
                    Level = 0
                });
            }
        }
    }

    private int GetBackgroundLevel(Guid backgroundId) =>
        CharacterBackgrounds.First(b => b.BackgroundId == backgroundId).Level;

    private void SetBackgroundLevel(Guid backgroundId, int value)
    {
        var bg = CharacterBackgrounds.First(b => b.BackgroundId == backgroundId);
        bg.Level = value;
    }

    private void OnBackgroundLevelChanged(Guid backgroundId, int newValue)
    {
        SetBackgroundLevel(backgroundId, newValue);
        ValidateTotalBackgroundPoints();
    }

    private int GetTotalBackgroundPointsUsed()
    {
        return CharacterBackgrounds.Sum(b => b.Level);
    }

    private void ValidateTotalBackgroundPoints()
    {
        var total = GetTotalBackgroundPointsUsed();

        if (total > MaxTotalPoints)
        {
            Snackbar.Add($"You have allocated too many points ({total}/{MaxTotalPoints}).", Severity.Error);
        }
    }

    public bool ValidateForm()
    {
        bool valid = true;
        var total = GetTotalBackgroundPointsUsed();

        if (total != MaxTotalPoints)
        {
            Snackbar.Add($"You must allocate exactly {MaxTotalPoints} points to backgrounds.", Severity.Error);
            valid = false;
        }
        if (total > MaxTotalPoints)
        {
            Snackbar.Add($"You have allocated too many points ({total}/{MaxTotalPoints}).", Severity.Error);
            valid = false;
        }

        return valid;
    }

    public async Task<bool> ValidateFormAsync()
    {
        await _formStepBackgrounds.Validate();
        return ValidateForm();
    }
}
