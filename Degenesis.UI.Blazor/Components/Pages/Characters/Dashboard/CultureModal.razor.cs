using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Characters.CRUD;
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

    protected override void OnParametersSet()
    {
        Culture.AvailableCults ??= [];
        Culture.BonusAttributes ??= [];
        Culture.BonusSkills ??= [];

        SelectedCultIds = [.. Culture.AvailableCults.Select(c => c.Id)];
        SelectedAttributeIds = [.. Culture.BonusAttributes.Select(a => a.Id)];
        SelectedSkillIds = [.. Culture.BonusSkills.Select(s => s.Id)];
    }

    private Task OnCultsChanged(IEnumerable<Guid> selectedValues)
    {
        SelectedCultIds = [.. selectedValues];
        Culture.AvailableCults = [.. Cults.Where(c => SelectedCultIds.Contains(c.Id))];
        return Task.CompletedTask;
    }

    private Task OnAttributesChanged(IEnumerable<Guid> selectedValues)
    {
        SelectedAttributeIds = [.. selectedValues];
        Culture.BonusAttributes = [.. Attributes.Where(a => SelectedAttributeIds.Contains(a.Id))];
        return Task.CompletedTask;
    }

    private Task OnSkillsChanged(IEnumerable<Guid> selectedValues)
    {
        SelectedSkillIds = [.. selectedValues];
        Culture.BonusSkills = [.. Skills.Where(s => SelectedSkillIds.Contains(s.Id))];
        return Task.CompletedTask;
    }

    private async Task SaveCulture()
    {
        HttpResponseMessage response;
        if (Culture.Id == Guid.Empty)
            response = await Client!.PostAsJsonAsync("/cultures", Culture);
        else
            response = await Client!.PutAsJsonAsync($"/cultures", Culture);

        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result<object>>();
            Snackbar.Add(result?.Error ?? "Unknown error", Severity.Error);
            return;
        }

        Snackbar.Add(Culture.Id == Guid.Empty ? "Created" : "Edited", Severity.Success);

        MudDialog.Close(DialogResult.Ok(true));
    }

    private void Cancel() => MudDialog.Cancel();
}
