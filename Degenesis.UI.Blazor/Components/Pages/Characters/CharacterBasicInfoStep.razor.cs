using Degenesis.Shared.DTOs.Characters;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Characters;

public partial class CharacterBasicInfoStep
{
    private MudForm _formStepBasic = default!;

    [Parameter] public CharacterCreateDto Character { get; set; } = new();
    [Parameter] public List<CultureDto> Cultures { get; set; } = [];
    [Parameter] public List<CultDto> AvailableCults { get; set; } = [];
    [Parameter] public List<ConceptDto> Concepts { get; set; } = [];

    [Parameter] public EventCallback<Guid> CultureChanged { get; set; }
    [Parameter] public EventCallback<Guid> CultChanged { get; set; }
    [Parameter] public EventCallback<Guid> ConceptChanged { get; set; }

    public bool IsValid { get; private set; } = false;

    public async Task<bool> ValidateFormAsync()
    {
        await _formStepBasic.Validate();

        bool selectsValid = Character.CultureId != Guid.Empty
                            && Character.CultId != Guid.Empty
                            && Character.ConceptId != Guid.Empty;

        IsValid = _formStepBasic.IsValid && selectsValid;
        if(!IsValid)
            Snackbar.Add("Please fill in all required fields and select Culture, Cult and Concept.", Severity.Error);
        return IsValid;
    }

    private async Task HandleCultureChange(Guid selectedCultureId)
    {
        Character.CultureId = selectedCultureId;
        await CultureChanged.InvokeAsync(selectedCultureId);
    }

    private async Task HandleCultChange(Guid selectedCultId)
    {
        Character.CultId = selectedCultId;
        await CultChanged.InvokeAsync(selectedCultId);
    }

    private async Task HandleConceptChange(Guid selectedConceptId)
    {
        Character.ConceptId = selectedConceptId;
        await ConceptChanged.InvokeAsync(selectedConceptId);
    }
}
