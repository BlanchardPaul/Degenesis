using Degenesis.Shared.DTOs.Characters;
using Degenesis.UI.Service.Features.Characters;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Characters;

public partial class ConceptModal
{
    [Inject] private ConceptService ConceptService { get; set; } = default!;
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;

    [Parameter] public ConceptDto Concept { get; set; } = new();
    [Parameter] public List<SkillDto> Skills { get; set; } = new();
    [Parameter] public List<AttributeDto> Attributes { get; set; } = new();

    private HashSet<Guid> SelectedBonusSkillIds { get; set; } = new();

    protected override void OnParametersSet()
    {
        Concept.BonusSkills ??= new List<SkillDto>();
        SelectedBonusSkillIds = Concept.BonusSkills.Select(bs => bs.Id).ToHashSet();

        if (Concept.BonusAttributeId == Guid.Empty && Attributes.Any())
        {
            Concept.BonusAttributeId = Attributes.First().Id;
        }
    }

    private Task OnBonusSkillsChanged(IEnumerable<Guid> selectedValues)
    {
        SelectedBonusSkillIds = selectedValues.ToHashSet();
        Concept.BonusSkills = Skills.Where(s => SelectedBonusSkillIds.Contains(s.Id)).ToList();
        return Task.CompletedTask;
    }

    private async Task SaveConcept()
    {
        if (Concept.Id == Guid.Empty)
            await ConceptService.CreateConceptAsync(Concept);
        else
            await ConceptService.UpdateConceptAsync(Concept);

        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel() => MudDialog.Cancel();
}
