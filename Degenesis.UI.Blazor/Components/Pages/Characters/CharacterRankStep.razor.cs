using Degenesis.Shared.DTOs.Characters.CRUD;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Characters;

public partial class CharacterRankStep
{
    [Parameter] public List<RankDto> Ranks { get; set; } = [];
    [Parameter] public CharacterCreateDto Character { get; set; } = new();
    private Guid SelectedRankId;

    private MudForm _formStepRank = default!;

    private Task OnRankSelected(Guid rankId)
    {
        SelectedRankId = rankId;
        Character.RankId = rankId;
        StateHasChanged();
        return Task.CompletedTask;
    }

    public bool ValidateForm()
    {
        if (Character.RankId == Guid.Empty)
        {
            Snackbar.Add("Please select a rank before continuing.", Severity.Error);
            return false;
        }
        return true;
    }

    public async Task<bool> ValidateFormAsync()
    {
        await _formStepRank.Validate();
        return ValidateForm();
    }
}
