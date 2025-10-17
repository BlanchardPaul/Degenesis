using Degenesis.Shared.DTOs.Characters.CRUD;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Characters.Dashboard;

public partial class ConceptModal
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;
    [Parameter] public ConceptDto Concept { get; set; } = new();
    [Parameter] public List<SkillDto> Skills { get; set; } = new();
    [Parameter] public List<AttributeDto> Attributes { get; set; } = new();
    private HttpClient _client = new();

    protected override async Task OnInitializedAsync()
    {
        _client = await HttpClientService.GetClientAsync();
    }

    private HashSet<Guid> SelectedBonusSkillIds { get; set; } = new();

    protected override void OnParametersSet()
    {
        Concept.BonusSkills ??= [];
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
        {
            var result = await _client.PostAsJsonAsync("/concepts", Concept);
            if (!result.IsSuccessStatusCode)
                Snackbar.Add("Error during creation", Severity.Error);
            else
            {
                Snackbar.Add("Created", Severity.Success);
                MudDialog.Close(DialogResult.Ok(true));
            }
        }

        else
        {
            var result = await _client.PutAsJsonAsync($"/concepts", Concept);
            if (!result.IsSuccessStatusCode)
                Snackbar.Add("Error during edition", Severity.Error);
            else
            {
                Snackbar.Add("Edited", Severity.Success);
                MudDialog.Close(DialogResult.Ok(true));
            }
        }
        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel() => MudDialog.Cancel();
}
