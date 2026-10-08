using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Characters.CRUD;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Characters.Dashboard;

public partial class ConceptModal
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;
    [Parameter] public ConceptDto Concept { get; set; } = new();
    [Parameter] public List<SkillDto> Skills { get; set; } = [];
    [Parameter] public List<AttributeDto> Attributes { get; set; } = [];
    private HashSet<Guid> SelectedBonusSkillIds { get; set; } = [];

    protected override void OnParametersSet()
    {
        Concept.BonusSkills ??= [];
        SelectedBonusSkillIds = [.. Concept.BonusSkills.Select(bs => bs.Id)];

        if (Concept.BonusAttributeId == Guid.Empty && Attributes.Count != 0)
        {
            Concept.BonusAttributeId = Attributes.First().Id;
        }
    }

    private Task OnBonusSkillsChanged(IEnumerable<Guid> selectedValues)
    {
        SelectedBonusSkillIds = [.. selectedValues];
        Concept.BonusSkills = [.. Skills.Where(s => SelectedBonusSkillIds.Contains(s.Id))];
        return Task.CompletedTask;
    }

    private async Task SaveConcept()
    {
        HttpResponseMessage response;
        if (Concept.Id == Guid.Empty)
            response = await Client!.PostAsJsonAsync("/concepts", Concept);
        else
            response = await Client!.PutAsJsonAsync($"/concepts", Concept);

        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result<object>>();
            Snackbar.Add(result?.Error ?? "Unknown error", Severity.Error);
            return;
        }

        Snackbar.Add(Concept.Id == Guid.Empty ? "Created" : "Edited", Severity.Success);

        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel() => MudDialog.Cancel();
}
