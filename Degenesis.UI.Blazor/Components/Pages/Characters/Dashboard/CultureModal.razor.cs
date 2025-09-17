using Degenesis.Shared.DTOs.Characters;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Characters.Dashboard;

public partial class CultureModal
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;

    [Parameter] public CultureDto Culture { get; set; } = new();
    [Parameter] public List<CultDto> Cults { get; set; } = [];
    [Parameter] public List<AttributeDto> Attributes { get; set; } = [];
    [Parameter] public List<SkillDto> Skills { get; set; } = [];

    private HashSet<Guid> SelectedCultIds { get; set; } = [];
    private HashSet<Guid> SelectedAttributeIds { get; set; } = [];
    private HashSet<Guid> SelectedSkillIds { get; set; } = [];
    private HttpClient _client = new();

    protected override async Task OnInitializedAsync()
    {
        _client = await HttpClientService.GetClientAsync();
    }
    protected override void OnParametersSet()
    {
        Culture.AvailableCults ??= [];
        Culture.BonusAttributes ??= [];
        Culture.BonusSkills ??= [];

        SelectedCultIds = Culture.AvailableCults.Select(c => c.Id).ToHashSet();
        SelectedAttributeIds = Culture.BonusAttributes.Select(a => a.Id).ToHashSet();
        SelectedSkillIds = Culture.BonusSkills.Select(s => s.Id).ToHashSet();
    }

    private Task OnCultsChanged(IEnumerable<Guid> selectedValues)
    {
        SelectedCultIds = selectedValues.ToHashSet();
        Culture.AvailableCults = Cults.Where(c => SelectedCultIds.Contains(c.Id)).ToList();
        return Task.CompletedTask;
    }

    private Task OnAttributesChanged(IEnumerable<Guid> selectedValues)
    {
        SelectedAttributeIds = selectedValues.ToHashSet();
        Culture.BonusAttributes = Attributes.Where(a => SelectedAttributeIds.Contains(a.Id)).ToList();
        return Task.CompletedTask;
    }

    private Task OnSkillsChanged(IEnumerable<Guid> selectedValues)
    {
        SelectedSkillIds = selectedValues.ToHashSet();
        Culture.BonusSkills = Skills.Where(s => SelectedSkillIds.Contains(s.Id)).ToList();
        return Task.CompletedTask;
    }

    private async Task SaveCulture()
    {
        if (Culture.Id == Guid.Empty)
        {
            var result = await _client.PostAsJsonAsync("/cultures", Culture);
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
            var result = await _client.PutAsJsonAsync($"/cultures", Culture);
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
